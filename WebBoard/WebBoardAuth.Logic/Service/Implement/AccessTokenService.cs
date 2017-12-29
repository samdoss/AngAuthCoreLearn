using WebBoardAuth.DataAccess.Sql;
using WebBoardAuth.DataAccess.Sql.Repository.Implement;
using WebBoardAuth.DataAccess.Sql.Repository.Interface;
//using WebBoardAuth.DataAccess.Redis.Repository.Implement;
//using WebBoardAuth.DataAccess.Redis.Repository.Interface;
using WebBoardAuth.Logic.Models;
using WebBoardAuth.Logic.Service.Interface;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebBoardAuth.Logic.Service.Implement
{
    public class AccessTokenService : IAccessTokenService
    {
        //private RedisUnitOfWork RedisUnitOfWork;
        AuthDbConnection AuthDbConnection;
        private ITokenRepository TokenRepository;


        //public AccessTokenService(RedisUnitOfWork _redisUnitOfWork)
        //{
        //    RedisUnitOfWork = _redisUnitOfWork;
        //}

        public AccessTokenService(AuthDbConnection _conn)
        {
            AuthDbConnection = _conn;
            TokenRepository = new TokenRepository(_conn);

        }

        public TimeSpan Expiration()
        {
            return TimeSpan.FromMinutes(30);
        }

        public async Task<object> PublishToken(Models.AccessTokenDto detail)
        {

            var now = detail.now;

            // Specifically add the jti (nonce), iat (issued timestamp), and sub (subject/user) claims.
            // You can add other claims here, if you want:
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, detail.Sub),
                new Claim(JwtRegisteredClaimNames.Jti, detail.Jti),
                new Claim(JwtRegisteredClaimNames.Iat, detail.Iat , ClaimValueTypes.Integer64),
                new Claim("roles",detail.Roles)
            };

            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                issuer: detail.Issuer,
                audience: detail.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(Expiration()),
                signingCredentials: detail.hashSecret);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)Expiration().TotalSeconds,
                refresh_token = Guid.NewGuid().ToString()
            };

            // Keep a access_token to tokensRepository (REDIS)
            int time_exp_in_redis = 14400; //unit of time is seconds
            var TokensForLogin = new TokensForLoginDto();
            TokensForLogin.Secret = detail.Secret;
            TokensForLogin.Issuer = detail.Issuer;
            TokensForLogin.Audience = detail.Audience;

            var str_secret = Convert.ToBase64String(detail.Secret);
            //mock detail.Roles = "admin+saleco+salemanager";
            await TokenRepository.SetValue(response.access_token, str_secret, time_exp_in_redis);  //await token_repo.SetValue(response.access_token, JsonConvert.SerializeObject(TokensForLogin, Formatting.None), time_exp_in_redis);

            // Keep a refresh_token to tokensRepository (REDIS)
            var TokensForRefresh = new TokensForRefreshDto();
            TokensForRefresh.access_token = response.access_token;
            TokensForRefresh.identity = "IDENTITY";
            await TokenRepository.SetValue(response.refresh_token, response.access_token, time_exp_in_redis); //await token_repo.SetValue(response.refresh_token, JsonConvert.SerializeObject(TokensForRefresh, Formatting.None), time_exp_in_redis);

            return response;
        }

        public async Task<bool> ValidateRefreshToken(string refresh_token, string access_token)
        {
            var check_token = await TokenRepository.GetValue(refresh_token);
            if (check_token.Equals(access_token))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<string> GetValueByToken(string access_token)
        {
            return await TokenRepository.GetValue(access_token);

        }
        public async Task<bool> IsExistsToken(string key)
        {
            return await TokenRepository.IsExists(key);

        }

        public async Task<bool> Delete(string key)
        {
            return await TokenRepository.Delete(key);
        }


    }
}
