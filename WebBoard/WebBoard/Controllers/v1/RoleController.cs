using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using WebBoardAuth.Logic;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebBoardWeb.Auth.Controllers.v1
{
    [EnableCors("AllowAll")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private ILogicUnitOfWork LogicUnitOfWork;

        public RoleController(RoleManager<IdentityRole> roleManager, ILogicUnitOfWork logicUnitOfWork)
            //: base(redisConnectionMultiplexer)
        {
            _roleManager = roleManager;
            LogicUnitOfWork = logicUnitOfWork;
        }

        [AllowAnonymous]
        [HttpPost("CreateRoleName")]
        public async Task<IActionResult> CreateRoleName([FromBody] string name)
        {
            var role = new IdentityRole { Name = name };
            var result = await _roleManager.CreateAsync(role);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("DeleteRoleName")]
        public async Task<IActionResult> DeleteRoleName([FromBody] string name)
        {
            var role = await _roleManager.FindByNameAsync(name);
            var result = await _roleManager.DeleteAsync(role);
            return Ok();
        }
    }
}
