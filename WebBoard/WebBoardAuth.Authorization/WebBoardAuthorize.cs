using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace WebBoardAuth.Authorization
{
    public class WebBoardAuthorize : ActionFilterAttribute
    {
        /*
         *********************** Setup Server Part ********************  
         */
        private string authServ = "http://localhost:59658/";
        private string redisServ = "localhost";

        /*
         ***************** Initial Global Variable *************** 
         */
        public string token_err = "token_error";
        public string token_detail = "";
        public int status_code = 200;
        private static string client_id;
        private static string secret_key;
        private static string role_auth;

        /*
         ********************** Class **********************  
         */
        public WebBoardAuthorize(string role_str)
        {
            role_auth = role_str;
        }

        public static void initialSecret(string id, string secret)
        {
            client_id = id;
            secret_key = secret;
        }
        /*
         ********************* Authorization Logic Part ******************** 
         */
        public static bool checkDebug()
        {
            var isDebug = Environment.GetEnvironmentVariable("isDebug");
            if (isDebug == "true")
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            token_err = "token_error";
            token_detail = "";

            //Initial Part
            var client_secret = secret_key;
            var role = role_auth;

            HttpRequest rq = context.HttpContext.Request;
            var header = rq.Headers;
            var response = context.HttpContext.Response;

            if (header.ContainsKey("token"))
            {
                Microsoft.Extensions.Primitives.StringValues r = "";
                var tk = header.TryGetValue("token", out r);

                var token = r.FirstOrDefault();
                if (tk && token.Length != 0 && token != null)
                {
                    /*
                     * success to get token from header
                     */
                    try
                    {
                        Redis rd = new Redis();
                        if (checkDebug())
                        {
                            rd = new Redis(authServ);
                        }
                        else
                        {
                            rd = new Redis(redisServ);
                        }

                        var secretKey = await rd.getSecret(token, client_secret, role);
                        if (secretKey == "token_error" || secretKey == null)
                        {
                            token_err = "token_error";
                            ChallengeAuthorization(response);
                            return;
                        }
                        /*
                         * If JwtCore.JsonWebToken.Decode not success it mean
                         * token has expired (timeout).
                         */
                        string jsonPayload = JwtCore.JsonWebToken.Decode(token, secretKey);

                        await next();
                    }
                    catch (Exception e)
                    {
                        token_err = "token_expired";
                        ChallengeAuthorization(response);
                        return;
                    }
                }
                else
                {
                    /*
                     * fail to get token from header
                     * token = null || length = 0
                     */
                    ChallengeAuthorization(response);
                    return;

                }
            }
            else if (header.ContainsKey("access") && header.ContainsKey("refresh"))
            {
                /*
                 * request for new token
                 */
                Microsoft.Extensions.Primitives.StringValues at = "";
                var ac = header.TryGetValue("access", out at);
                var access = at.FirstOrDefault();

                Microsoft.Extensions.Primitives.StringValues rt = "";
                var re = header.TryGetValue("refresh", out rt);
                var refresh = rt.FirstOrDefault();

                if (ac && re && access.Length != 0 && access != null && refresh.Length != 0 && refresh != null)
                {
                    /*
                     * request for new token
                     * with old access and refresh token
                     */
                    RefreshToken ref_token = new RefreshToken(authServ, redisServ);
                    token_detail = ref_token.GenerateNewToken(access, refresh);

                    ChallengeAuthorization(response);
                    return;
                }
                else
                {
                    /*
                     * fail to get access_token or refresh_token
                     * length = 0 || value is null
                     */
                    ChallengeAuthorization(response);
                    return;
                }

            }
            else
            {
                /*
                 * empty header
                 * not contain token or ( access_token and refresh_token )
                 */
                ChallengeAuthorization(response);
                return;
            }

        }

        /*
         ************************ Challenge Part *************************** 
         * do when unauthorize or return a new token
         */
        public string GenerateResponseMessage()
        {
            if (token_err == "token_expired")
            {
                /*status_code = 205*/
                status_code = StatusCodes.Status205ResetContent;
                return token_err;
            }
            else if (token_err == "token_error" && token_detail == "")
            {
                /*status_code = 204*/
                status_code = StatusCodes.Status204NoContent;
                return token_err;
            }
            else if (token_detail == "error" || token_detail == "")
            {
                /*status_code = 204*/
                status_code = StatusCodes.Status204NoContent;
                return token_detail;
            }
            else
            {
                /*status_code = 200*/
                status_code = StatusCodes.Status200OK;
                return token_detail;
            }
        }
        public void ChallengeAuthorization(HttpResponse Response)
        {
            var t = Task.Run(async () => { await ChallengeAuthRequest(Response); });
            t.Wait();
        }

        public async Task ChallengeAuthRequest(HttpResponse Response)
        {
            var t = token_err;
            var d = token_detail;

            var message = GenerateResponseMessage();
            var status = status_code;
            try
            {
                Response.StatusCode = status;
                await Response.WriteAsync(message, Encoding.UTF8);
            }
            catch (Exception e)
            {
                var error = e;
            }

        }

    }
}
