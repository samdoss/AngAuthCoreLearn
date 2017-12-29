using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebBoard.Controllers.TestApiAuth
{
    [Route("api/[controller]")]
    // [Authorize]
    [EnableCors("AllowAll")]
    //IF comment this below [Authorize("WebBoardAuthorize")] will be surely see a token  HttpContext.Request.Headers
    //[Authorize("WebBoardAuthorize")]
    public class ApiTestController : Controller
    {  
        [HttpPost("www")]
        public IActionResult Post([FromBody]DumModel value)
        {
            var context = HttpContext.Request.Headers;
            return Ok($"xxxx www { value } ");
        }

        public class DumModel {
            public string Value { get; set; }
        }
    }
}
