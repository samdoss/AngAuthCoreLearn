using WebBoardAuth.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebBoardAuth.Logic.Service.Interface
{
    public interface IAccessTokenService
    {
        /// The expiration time for the generated tokens. (Default is 5 minutes)
        TimeSpan Expiration();

        Task<object> PublishToken(AccessTokenDto detail);
        Task<bool> ValidateRefreshToken(string refresh_token, string access_token);

        Task<string> GetValueByToken(string access_token);
        Task<bool> IsExistsToken(string key);
        Task<bool> Delete(string key);
    }
}
