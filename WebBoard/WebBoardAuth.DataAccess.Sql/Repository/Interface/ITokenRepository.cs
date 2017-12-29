using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebBoardAuth.DataAccess.Sql.Repository.Interface
{
    public interface ITokenRepository
    {
        Task<string> GetValue(string key = "");
        Task<bool> SetValue(string key = "", string value = "", int time_exp = 0);
        Task<bool> Delete(string key = "");
        Task<bool> RenameKey(string old_key = "", string new_key = "");
        Task<bool> IsExists(string key = "");
        Task DeleteAll();
        void Dispose();
    }
}
