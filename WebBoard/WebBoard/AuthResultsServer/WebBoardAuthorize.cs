using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebBoard.AuthResultsServer
{
    
    public class WebBoardAuthorize
    {
        private readonly RequestDelegate _next;

        public WebBoardAuthorize(RequestDelegate next, TokenAuthOptions token)
        {
            _next = next;
            WebBoardAuth.Authorization.WebBoardAuthorize.initialSecret(token.Audience, token.Secret_key);
        }

        public Task Invoke(HttpContext httpContext)
        {
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class WebBoardAuthorizeExtensions
    {
        public static IApplicationBuilder UseWebBoardAuthorize(this IApplicationBuilder builder, TokenAuthOptions token)
        {
            return builder.UseMiddleware<WebBoardAuthorize>(token);
        }
    }
}
