using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WebBoardAuth.Logic;

namespace WebBoardAuth.Api.Controllers.v1
{
    public class ResetPasswordController : Controller
    {

        private ILogicUnitOfWork LogicUnitOfWork;

        public ResetPasswordController(ILogicUnitOfWork logicUnitOfWork)
            //: base(redisConnectionMultiplexer)
        {
            LogicUnitOfWork = logicUnitOfWork;
        }

        [HttpGet]
        public ContentResult Reset(string username, string token, string url)
        {
            // var requestUrl = HttpContext.Request.QueryString.ToString();
            var requestUrl = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(Request);
            requestUrl = requestUrl.Split('?')[1];

            var user_name = requestUrl.Split('&')[0];
            username = user_name.Substring(9);

            var tok = requestUrl.Split('&')[1];
            token = tok.Substring(6);

            var destination_url = requestUrl.Split('&')[2];
            url = destination_url.Substring(4);


            var html = System.IO.File.ReadAllText("Views/ResetPassword.html");
            //Replace Data for assign username & token
            html = html.Replace("emailrequest", username);
            html = html.Replace("tokenrequest", token);
            html = html.Replace("destinationrequest", url);

            return Content(html, "text/html");
        }

        [HttpGet("TEST")]
        public ContentResult TEST()
        {
            var html = System.IO.File.ReadAllText("Views/ResetPassword.html");
            //Replace Data for assign username & token
            html = html.Replace("emailrequest", "username");
            html = html.Replace("tokenrequest", "token");
            html = html.Replace("destinationrequest", "url");

            return Content(html, "text/html");

          
        }

        [HttpGet("Split")]
        public ContentResult Split(string username, string token, string url)
        {
            var requestUrl = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(Request);
            requestUrl = requestUrl.Split('?')[1];

            var user_name = requestUrl.Split('&')[0];
            username = user_name.Substring(9);

            var tok = requestUrl.Split('&')[1];
            token = tok.Substring(6);

            var destination_url = requestUrl.Split('&')[2];
            url = destination_url.Substring(4);

            return Content(requestUrl + " : " + username + " : " + token + " : " + url);
        }



    }
}
