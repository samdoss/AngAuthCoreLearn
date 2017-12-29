using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Cryptography;
using WebBoardAuth.Api.Models;
using Microsoft.AspNetCore.Cors;
using WebBoardAuth.Logic;
using Newtonsoft.Json;
using WebBoardAuth.Logic.Service.Interface;
using WebBoardAuth.Logic.Service.Implement;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebBoardWeb.Auth.Controllers.v1
{
   [EnableCors("AllowAll")]
    public class ValidationController : Controller
    {
        /**** Identity ****/

        private ILogicUnitOfWork LogicUnitOfWork;

        public ValidationController(ILogicUnitOfWork logicUnitOfWork)
            //: base(redisConnectionMultiplexer)
        {
            LogicUnitOfWork = logicUnitOfWork;
        }

        [HttpPost("CheckAccessibility")]
        public async Task<IActionResult> CheckAccessibility([FromBody] ValidationViewModel data)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Get secret from token in REDIS
            var str_result = await LogicUnitOfWork.AccessTokenService.GetValueByToken(data.Token);

            if (str_result == null)
            {
                return Unauthorized(); //token is not valid or Try to login again
            }
            else
            {
                var redis_secret = str_result.Split(',')[0];

                //check client secertkey
                if (data.ClientSecret != redis_secret)
                {
                    return Unauthorized();  //secertkey is not valid
                }
                else
                {
                    //check role
                    if(data.RoleRequire == null || data.RoleRequire == "")
                    {
                        return Ok("accessible");
                    }
                    else
                    {
                        var redis_role = str_result.Split(',')[1];
                        var list_redis_role = redis_role.Split('+').ToList();

                        var list_roleRequire = data.RoleRequire.Split(',').ToList();

                        for(int i =0; i < list_roleRequire.Count(); i++)
                        {
                            if (list_redis_role.IndexOf(list_roleRequire[i]) > -1 && data.RoleRequire != "")
                            {
                                return Ok("accessible");
                               
                            }
            
                        }
                   
                        return StatusCode(403); //your role can not access

                    }


                }

            }

        }

        [HttpPost("IsExists")]
        public async Task<bool> IsExists([FromBody] string key)
        {
            if (!ModelState.IsValid)
            {
                return false;
            }

            //Check bool Key in REDIS
            var result = LogicUnitOfWork.AccessTokenService.IsExistsToken(key);
            return await result;

        }

       



    }
}
