using WebBoardAuth.DataAccess.Sql.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace WebBoardAuth.DataAccess.Sql.Repository.Implement
{
    public class TokenRepository : ITokenRepository
    {

        AuthDbConnection AuthDbConnection;
        public TokenRepository(AuthDbConnection _conn)
        {
            AuthDbConnection = _conn;
        }

        public async Task<string> GetValue(string key = "")
        {
            string result = "";
            await AuthDbConnection.bakserverWebBoardAuthConn.OpenAsync();
            string sql = "SELECT Value FROM Token WHERE [Key] ='" + key + "'";
            SqlCommand cmd = new SqlCommand(sql, AuthDbConnection.bakserverWebBoardAuthConn);
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    result = reader.GetString(0);
                }
            }
            else
            {
                result = null;
            }
            AuthDbConnection.bakserverWebBoardAuthConn.Close();
            return result;
        }

        public async Task<bool> SetValue(string key = "", string value = "", int time_exp = 0)
        {
            //var aa = TimeSpan.FromSeconds(time_exp);
            await AuthDbConnection.bakserverWebBoardAuthConn.OpenAsync();
            string sql = "INSERT INTO Token([Key],Value,TimeExp) VALUES('" + key + "','" + value + "'," + time_exp + ")";
            SqlCommand cmd = new SqlCommand(sql, AuthDbConnection.bakserverWebBoardAuthConn);
            //cmd.Parameters.AddWithValue("@param1", key);
            //cmd.Parameters.AddWithValue("@param2", value);
            //cmd.Parameters.AddWithValue("@param3", time_exp);
            cmd.CommandType = CommandType.Text;

            await cmd.ExecuteNonQueryAsync();
            AuthDbConnection.bakserverWebBoardAuthConn.Close();
            return true;
        }

        public async Task<bool> Delete(string key = "")
        {
            await AuthDbConnection.bakserverWebBoardAuthConn.OpenAsync();
            string sql = "DELETE FROM Token WHERE [Key] ='" + key + "'";

            SqlCommand cmd = new SqlCommand(sql, AuthDbConnection.bakserverWebBoardAuthConn);
            await cmd.ExecuteNonQueryAsync();
            AuthDbConnection.bakserverWebBoardAuthConn.Close();
            return true;
        }

        public async Task<bool> RenameKey(string old_key = "", string new_key = "")
        {
            await AuthDbConnection.bakserverWebBoardAuthConn.OpenAsync();
            string sql = "UPDATE Token SET [Key] = '" + new_key + "' WHERE [Key] ='" + old_key + "'";

            SqlCommand cmd = new SqlCommand(sql, AuthDbConnection.bakserverWebBoardAuthConn);
            await cmd.ExecuteNonQueryAsync();
            AuthDbConnection.bakserverWebBoardAuthConn.Close();
            return true;
        }

        public async Task<bool> IsExists(string key = "")
        {
            await AuthDbConnection.bakserverWebBoardAuthConn.OpenAsync();
            string sql = "UPDATE Token SET TimeExp = " + 0 + " WHERE [Key] ='" + key + "'";

            SqlCommand cmd = new SqlCommand(sql, AuthDbConnection.bakserverWebBoardAuthConn);
            await cmd.ExecuteNonQueryAsync();
            AuthDbConnection.bakserverWebBoardAuthConn.Close();
            return true;
        }

        public async Task DeleteAll()
        {
            await AuthDbConnection.bakserverWebBoardAuthConn.OpenAsync();
            string sql = "DELETE FROM Token";

            SqlCommand cmd = new SqlCommand(sql, AuthDbConnection.bakserverWebBoardAuthConn);
            await cmd.ExecuteNonQueryAsync();
            AuthDbConnection.bakserverWebBoardAuthConn.Close();
        }


        public void Dispose()
        {
            AuthDbConnection.bakserverWebBoardAuthConn.Close();
        }
    }
}
