using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Net;
using Microsoft.AspNetCore.Cors;
using WebBoardAuth.Common.Utils;
using System.Data.SqlClient;
using WebBoardAuth.Logic.Service.Interface;
using WebBoardAuth.Logic.Models;
using WebBoardAuth.Logic.Service.Implement;
using WebBoardAuth.Logic;
using WebBoardAuth.Api.Models.AccountModel;
using WebBoardAuth.Api.Models;
using WebBoardAuth.Api.Models.ExternalAccountViewModel;

namespace WebBoardAuth.Api.Controllers.v1
{
    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        //private IAccessTokenService GenerateAccessToken { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private ILogicUnitOfWork LogicUnitOfWork;
        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, ILogicUnitOfWork logicUnitOfWork)
            //: base(redisConnectionMultiplexer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            LogicUnitOfWork = logicUnitOfWork;
            //GenerateAccessToken = new AccessTokenService();
        }

        // api/Account/Register
        [AllowAnonymous]
        [HttpGet("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel data)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            /*** Register User to table ***/
            data = new RegisterViewModel();
            data.ConfirmPassword = "123123123";
            data.Password = "123123123";
            data.UserName = "123123123";

            if (data.Password == data.ConfirmPassword)
            {
                //var dto = new RegisterDto();
                //dto.UserName = data.UserName;
                //dto.Password = data.Password;
                //dto.ConfirmPassword = data.ConfirmPassword;
                var user = new ApplicationUser { UserName = data.UserName };
                var result = await _userManager.CreateAsync(user, data.Password);
                /*** Get error result if it can't register ***/
                if (!result.Succeeded) return GetErrorResult(result);
                var prepare_id = await _userManager.FindByNameAsync(data.UserName);
                /*** Return 200 when it not has error ***/
                return Ok(prepare_id.Id);
            }
            return BadRequest();
        }

        // api/Account/CheckRegister
        [AllowAnonymous]
        [HttpGet("CheckRegister")]
        public async Task<IActionResult> CheckRegister([FromBody] string username)
        {
            username = "123123123";
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            /*** check registration User in table ***/
            var result = await _userManager.FindByNameAsync(username);
            if (result != null) return Ok(result.Id); //This username has already been taken.
            return Ok(1); //This username can be used.
        }

        // api/Account/CheckRegisterByID
        [AllowAnonymous]
        [HttpGet("CheckRegisterByID")]
        public async Task<IActionResult> CheckRegisterByID([FromBody] string user_id)
        {
            user_id = "fdbb2400-861d-4bb8-a1ee-31a1a1965b1d";
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            /*** check registration User in table ***/
            var result = await _userManager.FindByIdAsync(user_id);
            if (result != null) return Ok(result.UserName);
            return BadRequest();
        }

        // api/Account/PreActivate
        [AllowAnonymous]
        [HttpGet("PreActivate")]
        public async Task<IActionResult> PreActivate([FromBody] string username)
        {
            username = "123123123";
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByNameAsync(username);
            /*** Check activation of account ***/
            if(!await _userManager.IsEmailConfirmedAsync(user))
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                token = WebUtility.UrlEncode(token);
                return Ok(token);
            }
            /*** Return 200 and some text when username has activated ***/
            return Ok("Your username has activated.");
        }

        // api/Account/PostActivate
        [AllowAnonymous]
        [HttpGet("PostActivate")]
        public async Task<IActionResult> PostActivate([FromBody] ActivateViewModel data)
        {
            data = new ActivateViewModel();
            data.Username = "123123123";
            data.Token = "CfDJ8OmaK70eeZJEvHOZzinDgyBwTbCKh4i9AZlF6SNmIa%2BA9hYetipj1GIIFP0JCouy6WLjbCuArknGyK4tpvbNTicRpcJBydDqtCKqzg52Q%2BOBErLIJhtClyB2wi%2FFzJY5xeR2QevOH2Lub5dKiQzGwEOYdegj3YmrV88Iz6ZXTQDnENM06xR6Samx89qr2tCKx7AkokpMdOWYYfybvImtgNb6A84Qg0ggtYfnZIavZMXQL1FA4w%2Fc3Wb2oAyInjbYEA%3D%3D";
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByNameAsync(data.Username);
            /*** Check activation of account ***/
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                data.Token = WebUtility.UrlDecode(data.Token);
                var result = await _userManager.ConfirmEmailAsync(user,data.Token);
                /*** Get error result if it can't activate ***/
                if (!result.Succeeded) return GetErrorResult(result);
                return Ok("Username successfully activated"); // DISPLAYED AT THE FIRST TIME WHEN POST ACTIVATED SUCCESSFULLY
            }
            /*** Return 200 and some text when username has activated ***/
            return Ok("Your username has activated.");
        }

        // api/Account/ChangePassword
        [AllowAnonymous]
        [HttpGet("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByNameAsync(data.UserName);
            /*** Change password of user in table ***/
            var result = await _userManager.ChangePasswordAsync(user, data.OldPassword, data.NewPassword);
            /*** Get error result if it can't change password ***/
            if (!result.Succeeded) return GetErrorResult(result);
            /*** Return 200 when it not has error ***/
            return Ok("Your password has been changed successfully"); // DISPLAYED AT THE FIRST TIME WHEN PASSWORD CHANGED SUCCESSFULLY
        }

        // api/Account/PreResetPassword
        [AllowAnonymous]
        [HttpGet("PreResetPassword")]
        public async Task<IActionResult> PreResetPassword([FromBody]PreResetPasswordViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByNameAsync(data.UserName);
            /*** Generate token of account ***/
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            //var hostname = HttpContext.Request.Host.Value;
            //var model = new ForgotPasswordDto();
            //model.code = token;
            //model.email = user.UserName;
            //model.hostname = hostname;
            //model.fromurl = data.FromUrl;
            //await LogicUnitOfWork.EmailService.SendEmailForgotPassword(model);
            
            return Ok(token);
        }


        // api/Account/PostResetPassword
        [AllowAnonymous]
        [HttpGet("PostResetPassword")]
        public async Task<IActionResult> PostResetPassword([FromBody] ResetPasswordViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByNameAsync(data.Email);
            
            /*** Set new password of account ***/
            var result = await _userManager.ResetPasswordAsync(user,data.Token,data.Password);
            /*** Get error result if it can't reset password ***/
            if (!result.Succeeded) return GetErrorResult(result);
            /*** Return 200 ***/
            return Ok("Your password has been changed successfully"); // DISPLAYED AT THE FIRST TIME WHEN RESET PASSWORD SUCCESSFULLY
        }

        // api/Account/Lock
        [AllowAnonymous]
        [HttpGet("Lock")]
        public async Task<IActionResult> Lock([FromBody] SetLockViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByNameAsync(data.UserName);
            /*** Lock User in table ***/
            var result = await _userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.UtcNow).AddMinutes(data.Minutes));
            await _userManager.ResetAccessFailedCountAsync(user);
            /*** Get error result if it can't lock ***/
            if (!result.Succeeded) return GetErrorResult(result);
            /*** Return 200 when it not has error ***/
            return Ok("User has been locked successfully"); // DISPLAYED WHEN USER HAS BEEN LOCKED SUCCESSFULLY
        }

        // api/Account/Unlock
        [AllowAnonymous]
        [HttpGet("Unlock")]
        public async Task<IActionResult> Unlock([FromBody] string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByNameAsync(username);
            /*** Unlock User in table ***/
            var result = await _userManager.SetLockoutEndDateAsync(user, null);
            await _userManager.ResetAccessFailedCountAsync(user);
            /*** Get error result if it can't unlock ***/
            if (!result.Succeeded) return GetErrorResult(result);
            /*** Return 200 when it not has error ***/
            return Ok("User has been unlocked successfully"); // DISPLAYED WHEN USER HAS BEEN UNLOCKED SUCCESSFULLY
        }

        // api/Account/Logout
        [AllowAnonymous]
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout([FromBody] TokensViewModel token)
        {
            try
            {
                /*** Get logout every CookieAuthenticationScheme ***/
                await _signInManager.SignOutAsync();
                /*** Delete token every types ***/
                await LogicUnitOfWork.AccessTokenService.Delete(token.Access);  // Delete access token
                await LogicUnitOfWork.AccessTokenService.Delete(token.Refresh); // Delete refresh token
            }
            catch (Exception e)
            {
                /*** Return 400 when it has error ***/
                return BadRequest(e);
            }
            /*** Return 200 when it not has error ***/
            return Ok("You have successfully logged out"); // DISPLAYED WHEN USER HAS SUCCESSFULLY LOGGED OUT
        }

        // api/Account/Delete
        [AllowAnonymous]
        [HttpGet("Delete")]
        public async Task<IActionResult> Delete([FromBody] RegisterViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var username = await _userManager.FindByNameAsync(data.UserName);

            /*** confirm delete by check password ***/
            if (data.Password == data.ConfirmPassword)
            {
                if (await _userManager.CheckPasswordAsync(username, data.Password))
                {
                    /*** Delete user in table ***/
                    var result = await _userManager.DeleteAsync(username);
                    /*** Get error result if it can't delete ***/
                    if (!result.Succeeded) return GetErrorResult(result);
                    /*** Return 200 when it not has error ***/
                    return Ok("This user has been deleted successfully"); // DISPLAYED WHEN USER HAS BEEN DELETED SUCCESSFULLY
                }
            }
            return BadRequest();
        }

        /******************* External Login Section *************************/

        // api/Account/ExternalLogin
        [AllowAnonymous]
        [HttpGet("ExternalLogin")]
        public IActionResult ExternalLogin([FromBody] ExternalLoginViewModel data)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = data.ReturnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(data.Provider, redirectUrl);
            return Challenge(properties, data.Provider);
        }

        // api/Account/ExternalLoginCallback
        [AllowAnonymous]
        [HttpGet("ExternalLoginCallback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return BadRequest(ModelState);
            }

            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            /*** Return 400 when can't get externalLoginInfo ***/
            if (info == null) return BadRequest("Please Login");

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded) {
                return Ok("Redirect to homepage after login");
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var user = await _userManager.FindByNameAsync(email);
                var login = new IdentityResult();
                if (user != null)
                {
                    /*** Check activation of account ***/
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        /*** Do Confirm email ***/
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmEmail = await _userManager.ConfirmEmailAsync(user, token);
                        /*** Get error result if it can't confirm email ***/
                        if (!confirmEmail.Succeeded) return GetErrorResult(confirmEmail);
                    }
                    /*** Do AddLoginProvider ***/
                    login = await _userManager.AddLoginAsync(user, info);
                    /*** Get error result if it can't add provider ***/
                    if (!login.Succeeded) return GetErrorResult(login);
                    /*** Do SignIn ***/
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    /*** Generate Token ***/
                    var response = await LogicUnitOfWork.AccessTokenService.PublishToken(await PrepareGenerateTokenExternalLogin(email));
                    return Ok(response);
                    
                }
                else
                {
                    return Ok("Register");
                }
            }
        }

        // api/Account/ExternalLoginToRegister
        [AllowAnonymous]
        [HttpGet("ExternalLoginToRegister")]
        public async Task<IActionResult> ExternalLoginToRegister([FromBody] ExternalLoginToRegisterViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            /*** Return 400 when can't get externalLoginInfo ***/
            if (info == null) return BadRequest("Can't get externalLoginInfo");
            /*** Do Register ***/
            var user = new ApplicationUser { UserName = data.Email, Email = data.Email };
            var result = await _userManager.CreateAsync(user,data.Password);
            /*** Get error result if it can't register ***/
            if (!result.Succeeded) return GetErrorResult(result);
            /*** Do AddLoginProvider ***/
            result = await _userManager.AddLoginAsync(user, info);
            /*** Get error result if it can't add provider ***/
            if (!result.Succeeded) return GetErrorResult(result);
            /*** Do SignIn ***/
            await _signInManager.SignInAsync(user, isPersistent: false);
            /*** Generate Token ***/
            var response = await LogicUnitOfWork.AccessTokenService.PublishToken(await PrepareGenerateTokenExternalLogin(data.Email));
            return Ok(response);
        }

        /******************************************************************/
        private IActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null) return StatusCode(500); //InternalServerError
            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }
                // No ModelState errors are available to send, so just return an empty BadRequest.
                if (ModelState.IsValid) return BadRequest();
                return BadRequest(ModelState);
            }
            return null;
        }

        private async Task<AccessTokenDto> PrepareGenerateTokenExternalLogin(string username)
        {
            AccessTokenDto Detail = new AccessTokenDto();
            //Detail.Secret = Guid.NewGuid().ToString();
            //Detail.hashSecret = await Startup.GetHashSecret(Detail.Secret);
            Detail.Sub = username;
            Detail.Jti = await Task.FromResult(Guid.NewGuid().ToString());
            Detail.Iat = DateTimeUtils.ToUnixEpochDate(Detail.now).ToString();
            Detail.Issuer = "ExampleIssuer";
            Detail.Audience = "ExampleAudience";
            return Detail;
        }


        // api/Account/MigrateUser
        [AllowAnonymous]
        [HttpGet("MigrateUser")]
        public async Task<IActionResult> MigrateUser()
        {
            var data = new RegisterViewModel();
            SqlConnection connection = new SqlConnection("Data Source=DESKTOP-KR0EJIO;Initial Catalog=WebBoarddb;Persist Security Info=True;User ID=WebBoardsa;Password=C()nstructi0nUn!4ed");
            SqlCommand command = new SqlCommand("SELECT user_email, user_amphur FROM T_User WHERE user_amphur IS NOT NULL;", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    data.UserName = reader.GetString(0);
                    data.Password = reader.GetString(1);
                    data.ConfirmPassword = data.Password;
                    var user = new ApplicationUser { UserName = data.UserName };
                    var result = await _userManager.CreateAsync(user, data.Password);
                }
            }
            connection.Close();
            return Ok();
        }
    }
}