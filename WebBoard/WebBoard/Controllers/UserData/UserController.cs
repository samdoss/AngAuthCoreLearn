using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebBoard.Controllers.UserData
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        [HttpGet("getuserdata")]
        public object Get(string email)
        {
            var dummuser = new
            {
                UserId = 1,
                Titlename = "",
                Firstname = "",
                Lastname = "",
                Phone = "",
                Email = "",
            };
            return dummuser;
        }
         
    }
}
