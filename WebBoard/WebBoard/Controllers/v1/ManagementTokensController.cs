using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebBoardAuth.Api.Models;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Cors;
using WebBoardAuth.Logic;

namespace WebBoardAuth.Api.Controllers.v1
{
    [EnableCors("AllowAll")]
    public class ManagementTokensController : Controller
    {

        private ILogicUnitOfWork LogicUnitOfWork;
        public ManagementTokensController(ILogicUnitOfWork logicUnitOfWork)
           // : base(redisConnectionMultiplexer)
        {
            LogicUnitOfWork = logicUnitOfWork;
        }

        [AllowAnonymous]
        [HttpPost("CheckValidateTokens")]
        public async Task<IActionResult> CheckValidateTokens([FromBody] string access_token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Get secret from token in REDIS
            var detail_token_json = await LogicUnitOfWork.AccessTokenService.GetValueByToken(access_token);
            try
            {
                var secret = detail_token_json; //JsonConvert.DeserializeObject<TokensForLoginViewModel>(detail_token_json).Secret;
                var issuer = "ExampleIssuer";   //JsonConvert.DeserializeObject<TokensForLoginViewModel>(detail_token_json).Issuer;
                var audience = "ExampleAudience"; //JsonConvert.DeserializeObject<TokensForLoginViewModel>(detail_token_json).Audience;
                var tokenValidationParameters = new TokenValidationParameters
                {
                    // The signing key must match!
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                    // Validate the JWT Issuer (iss) claim
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    // Validate the JWT Audience (aud) claim
                    ValidateAudience = true,
                    ValidAudience = audience,
                    // Validate the token expiry
                    ValidateLifetime = true,
                    // If you want to allow a certain amount of clock drift, set that here:
                    ClockSkew = TimeSpan.Zero
                };
                SecurityToken securityToken;
                new JwtSecurityTokenHandler().ValidateToken(access_token, tokenValidationParameters, out securityToken); // Check validate access token with secret
                return Ok("OK");
            }
            catch
            {
                return BadRequest("Not found");
            }

        }

        [AllowAnonymous]
        [HttpPost("ClearTokens")]
        public async Task<IActionResult> ClearTokens([FromBody] TokensViewModel token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Delete access token and refresh token in REDIS
            try
            {
                await LogicUnitOfWork.AccessTokenService.Delete(token.Access);  // Delete access token
                await LogicUnitOfWork.AccessTokenService.Delete(token.Refresh); // Delete refresh token
            }
            catch
            {
                return BadRequest();  // send bad request if deletion token fail
            }
            return Ok();
        }
    }
}
