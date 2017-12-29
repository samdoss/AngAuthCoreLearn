using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using WebBoardWeb.Auth.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using WebBoardAuth.Common.Utils;
using WebBoardAuth.Api;
using WebBoardAuth.Common.Enums;
using WebBoardAuth.Logic.Service.Interface;
using WebBoardAuth.Logic.Models;
using WebBoardAuth.Logic.Service.Implement;
using WebBoardAuth.Logic;
using WebBoardAuth.Api.Models.AccountModel;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebBoardWeb.Auth.Controllers.v1
{

    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        /**** Identity ****/

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly TokenProviderOptions _options;
        private readonly JsonSerializerSettings _serializerSettings;
        private ILogicUnitOfWork LogicUnitOfWork;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogicUnitOfWork logicUnitOfWork)
        //:base(redisConnectionMultiplexer)
        {
            //_logger = loggerFactory.CreateLogger<TokenProviderMiddleware>();

            _options = new TokenProviderOptions
            {
                Audience = GetAudience,
                Issuer = GetIssuer,
                SigningCredentials = GetHashSecret
            };

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
            LogicUnitOfWork = logicUnitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public static Task<SigningCredentials> GetHashSecret(byte[] secretKey)
        {
            //var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            //return Task.FromResult(new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
            return Task.FromResult(new SigningCredentials(new SymmetricSecurityKey(secretKey), "HS256"));
        }

        private Task<string> GetIssuer(string Issuer)
        {
            return Task.FromResult(Issuer);
        }

        private Task<string> GetAudience(string Audience)
        {
            return Task.FromResult(Audience);
        }

        //private AuthViewModel dummyrefresh()
        //{
        //    AuthViewModel model = new AuthViewModel()
        //    {
        //        UserName = "123123123",
        //        Password = "123123123",
        //        ClientId = "03162ea57ec34b578f1190a9d783d9df",
        //        GrantType = "refresh",
               
        //    };
        //    return model;
        //}

        //private AuthViewModel dummypassword()
        //{
        //    AuthViewModel model = new AuthViewModel()
        //    {
        //        UserName = "123123123",
        //        Password = "123123123",
        //        ClientId = "03162ea57ec34b578f1190a9d783d9df",
        //        GrantType = "password"
        //    };
        //    return model;
        //}
        // Authentication Endpoint
        [HttpPost("RequestToken")]
        // api/Auth/RequestToken
        public async Task<IActionResult> RequestToken([FromBody]AuthViewModel model)
        {
            //model = dummyrefresh();
            var context = HttpContext;
            //_logger.LogInformation("Handling request: " + HttpContext.Request.Path);

            if (model.GrantType.Equals("password"))
            {
                return await GenerateToken(context, model);
            }
            else if (model.GrantType.Equals("refresh"))
            {
                return await GenerateTokenFromRefresh(context, model);
            }
            else if (model.GrantType.Equals("bypass"))
            {
                return await GenerateTokenFromBypass(context, model);
            }

            return BadRequest("Authentication method not supported.");
        }

        private async Task<IActionResult> GenerateToken(HttpContext context, AuthViewModel model)
        {
            string clientSecret = string.Empty;
            var detail_user = await _userManager.FindByNameAsync(model.UserName);

            Microsoft.AspNetCore.Identity.SignInResult identity;
            try
            {
                var login = new LoginViewModel(model.UserName, model.Password);
                identity = await _signInManager.PasswordSignInAsync(detail_user, login.Password, login.RememberMe, lockoutOnFailure: true);
            }
            catch
            {
                return BadRequest("Invalid username or password.");
            }

            if (identity.Succeeded)
            {
                if (await _userManager.IsEmailConfirmedAsync(detail_user))
                {
                    if (identity.IsNotAllowed)
                    {
                        return Ok("Your username isn't allowed.");
                    }
                }
                else
                {
                    return Ok("Your username isn't activated.");
                }
            }
            else
            {
                if (identity.IsLockedOut)
                {
                    var date = await _userManager.GetLockoutEndDateAsync(detail_user);
                    return Ok("Login again before " + date.ToString() + ".");
                }
                else
                {
                    return BadRequest("Invalid username or password.");
                }
            }

            if (model.ClientId == null)
            {
                return BadRequest("client_Id is not set");
            }
            var audience = await LogicUnitOfWork.AudienceService.FindAudience(model.ClientId);
            if (audience == null)
            {
                return BadRequest("Invalid client_id " + model.ClientId);
            }
            var client = await LogicUnitOfWork.ClientService.FindClient(model.ClientId);
            if (client == null)
            {
                return BadRequest("Client " + model.ClientId + " is not registered in the system.");
            }
            if (client.ApplicationType == ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    return BadRequest("Client secret should be sent.");
                }
                else
                {
                    if (client.Secret != Helper.GetHash(clientSecret))
                    {
                        return BadRequest("Client secret is invalid.");
                    }
                }
            }
            if (!client.IsActive)
            {
                return BadRequest("Client is inactive.");
            }

            /***** Logic for get role *****/
            var user = await _userManager.FindByNameAsync(model.UserName);
            var list_role = await _userManager.GetRolesAsync(user);
            string role = "";

            if (list_role.Count == 1)
            {
                foreach (var i in list_role)
                {
                    role = i;
                }
            }
            else
            {
                int c = 1;
                foreach (var i in list_role)
                {
                    role = role + i;
                    if (list_role.Count > c)
                    {
                        role = role + ",";
                        c++;
                    }
                }
                role = "[" + role + "]";
            }


            /***** Logic for token *****/
            AccessTokenDto Detail = new AccessTokenDto();
            //Detail.Secret = Guid.NewGuid().ToString();
            Detail.Secret = Convert.FromBase64String(audience.Base64Secret);
            Detail.hashSecret = await _options.SigningCredentials(Detail.Secret);
            Detail.Sub = model.UserName;
            Detail.Jti = await _options.NonceGenerator();
            Detail.Iat = DateTimeUtils.ToUnixEpochDate(Detail.now).ToString();
            Detail.Issuer = await _options.Issuer(audience.Issuer);
            Detail.Audience = await _options.Audience(audience.ClientId);
            Detail.Roles = role;

            var response = await LogicUnitOfWork.AccessTokenService.PublishToken(Detail);
            return Ok(response);

        }

        private async Task<IActionResult> GenerateTokenFromRefresh(HttpContext context, AuthViewModel model)
        {
            var access_token = model.Access;
            var refresh_token = model.Refresh;
            string clientSecret = string.Empty;

            if (!await LogicUnitOfWork.AccessTokenService.ValidateRefreshToken(refresh_token, access_token))
            {
                return BadRequest("Invalid access token");
            }

            //var check_identity = JsonConvert.DeserializeObject<TokensForRefreshModel>(detail_refresh_json).identity;
            //if (!check_identity.Equals(identityfromdatabase))
            //{
            //    context.Response.StatusCode = 400;
            //    await context.Response.WriteAsync("Invalid identity");
            //    return;
            //}
            if (model.ClientId == null)
            {
                return BadRequest("client_Id is not set");
            }
            var audience = await LogicUnitOfWork.AudienceService.FindAudience(model.ClientId);
            if (audience == null)
            {
                return BadRequest("Invalid client_id " + model.ClientId);
            }
            var client = await LogicUnitOfWork.ClientService.FindClient(model.ClientId);
            if (client == null)
            {
                return BadRequest("Client " + model.ClientId + " is not registered in the system.");
            }
            if (client.ApplicationType == ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    return BadRequest("Client secret should be sent.");
                }
                else
                {
                    if (client.Secret != Helper.GetHash(clientSecret))
                    {
                        return BadRequest("Client secret is invalid.");
                    }
                }
            }
            if (!client.IsActive)
            {
                return BadRequest("Client is inactive.");
            }
            var payload = new JwtSecurityTokenHandler().ReadJwtToken(access_token).Payload;
            var secret = Convert.FromBase64String(audience.Base64Secret);

            //var payload = new JwtSecurityTokenHandler().ReadJwtToken(access_token).Payload;
            //var secret = Guid.NewGuid().ToString();


            /***** Logic for get role *****/
            var user = await _userManager.FindByNameAsync(model.UserName);
            var list_role = await _userManager.GetRolesAsync(user);

            string role = "";
            if (list_role.Count == 1)
            {
                foreach (var i in list_role)
                {
                    role = i;
                }
            }
            else
            {
                int c = 1;
                foreach (var i in list_role)
                {
                    role = role + i;
                    if (list_role.Count > c)
                    {
                        role = role + ",";
                        c++;
                    }
                }
                role = "[" + role + "]";
            }


            AccessTokenDto Detail = new AccessTokenDto();
            Detail.Secret = secret;
            Detail.hashSecret = await _options.SigningCredentials(Detail.Secret);
            Detail.Sub = payload.Sub;
            Detail.Jti = payload.Jti;
            Detail.Iat = payload.Iat.ToString();
            Detail.Issuer = payload.Iss;
            Detail.Audience = payload.Aud[0];
            Detail.Roles = role;
            var response = await LogicUnitOfWork.AccessTokenService.PublishToken(Detail);

            // Serialize and return the response
            //return Ok(JsonConvert.SerializeObject(response, _serializerSettings));
            return Ok(response);
        }

        private async Task<IActionResult> GenerateTokenFromBypass(HttpContext context, AuthViewModel model)
        {
            var username = model.UserName;
            var key = model.Key;
            var checksum = model.CheckSum;
            var calChecksum = GenerateChecksum(key);
            string clientSecret = string.Empty;

            if (calChecksum != checksum)
            {
                return BadRequest("Error!");
            }

            var detail_user = await _userManager.FindByNameAsync(username);
            if (detail_user == null)
            {
                return BadRequest("User not found.");
            }
            if (model.ClientId == null)
            {
                return BadRequest("client_Id is not set");
            }
            var audience = await LogicUnitOfWork.AudienceService.FindAudience(model.ClientId);
            if (audience == null)
            {
                return BadRequest("Invalid client_id " + model.ClientId);
            }
            var client = await LogicUnitOfWork.ClientService.FindClient(model.ClientId);
            if (client == null)
            {
                return BadRequest("Client " + model.ClientId + " is not registered in the system.");
            }
            if (client.ApplicationType == ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    return BadRequest("Client secret should be sent.");
                }
                else
                {
                    if (client.Secret != Helper.GetHash(clientSecret))
                    {
                        return BadRequest("Client secret is invalid.");
                    }
                }
            }
            if (!client.IsActive)
            {
                return BadRequest("Client is inactive.");
            }


            /***** Logic for get role *****/
            var user = await _userManager.FindByNameAsync(model.UserName);
            var list_role = await _userManager.GetRolesAsync(user);

            string role = "";
            if (list_role.Count == 1)
            {
                foreach (var i in list_role)
                {
                    role = i;
                }
            }
            else
            {
                int c = 1;
                foreach (var i in list_role)
                {
                    role = role + i;
                    if (list_role.Count > c)
                    {
                        role = role + ",";
                        c++;
                    }
                }
                role = "[" + role + "]";
            }

            /***** Logic for token *****/
            AccessTokenDto Detail = new AccessTokenDto();
            ////Detail.Secret = Guid.NewGuid().ToString();
            Detail.Secret = Convert.FromBase64String(audience.Base64Secret);
            Detail.hashSecret = await _options.SigningCredentials(Detail.Secret);
            Detail.Sub = username;
            Detail.Jti = await _options.NonceGenerator();
            Detail.Iat = DateTimeUtils.ToUnixEpochDate(Detail.now).ToString();
            Detail.Issuer = await _options.Issuer(audience.Issuer);
            Detail.Audience = await _options.Audience(audience.ClientId);
            Detail.Roles = role;
            var response = await LogicUnitOfWork.AccessTokenService.PublishToken(Detail);

            // Serialize and return the response
            return Ok(JsonConvert.SerializeObject(response, _serializerSettings));
        }
        private static string GenerateChecksum(string key)
        {
            string secretPhase = "eXVpY3JlYXRlcmFpbnN0b3Jtd2hlbnNoZXJ1bg==";
            SHA256 sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(string.Format("{0}{1}", key, secretPhase));
            var hash = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hash);
        }
    }
}
