using WebBoardAuth.Common.Cryptography;
using WebBoardAuth.Common.Enums;
using WebBoardAuth.DataAccess.Sql;
using WebBoardAuth.DataAccess.Sql.Entities;
using WebBoardAuth.Logic.Models;
using WebBoardAuth.Logic.Service.Interface;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace WebBoardAuth.Logic.Service.Implement
{
    public class ClientService : IClientService
    {
        AuthDbConnection AuthDbConnection;

        public ClientService(AuthDbConnection _conn)
        {
            AuthDbConnection = _conn;
        }

        public async Task AddClient(ClientDto model)
        {
            var obj = new Client()
            {
                Id = model.Id,
                Secret = SHA256Cryptographer.GetHash(model.Id),
                Name = model.Name,
                ApplicationType = ApplicationTypes.JavaScript,
                IsActive = true,
                RefreshTokenLifeTime = 14400,
                AllowedOrigin = "*"
            };
            await AuthDbConnection.bakserverWebBoardAuthConn.OpenAsync();
            string sql = "INSERT INTO Clients(Id,Secret,Name,ApplicationType,IsActive,RefreshTokenLifeTime,AllowedOrigin) VALUES(@param1,@param2,@param3,@param4,@param5,@param6,@param7)";
            SqlCommand cmd = new SqlCommand(sql, AuthDbConnection.bakserverWebBoardAuthConn);
            cmd.Parameters.Add("@param1", SqlDbType.VarChar).Value = obj.Id;
            cmd.Parameters.Add("@param2", SqlDbType.VarChar).Value = obj.Secret;
            cmd.Parameters.Add("@param3", SqlDbType.VarChar).Value = obj.Name;
            cmd.Parameters.Add("@param4", SqlDbType.Int).Value = obj.ApplicationType;
            cmd.Parameters.Add("@param5", SqlDbType.Bit).Value = obj.IsActive;
            cmd.Parameters.Add("@param6", SqlDbType.Int).Value = obj.RefreshTokenLifeTime;
            cmd.Parameters.Add("@param7", SqlDbType.VarChar).Value = obj.AllowedOrigin;
            cmd.CommandType = CommandType.Text;
            await cmd.ExecuteNonQueryAsync();
            AuthDbConnection.bakserverWebBoardAuthConn.Close();
        }
        public async Task<Client> FindClient(string clientId)
        {
            Client client = new Client();
            await AuthDbConnection.bakserverWebBoardAuthConn.OpenAsync();
            string sql = "SELECT * FROM Clients WHERE Id ='" + clientId + "'";
            SqlCommand cmd = new SqlCommand(sql, AuthDbConnection.bakserverWebBoardAuthConn);
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    client.Id = reader.GetString(0);
                    client.Secret = reader.GetString(1);
                    client.Name = reader.GetString(2);
                    client.ApplicationType = (ApplicationTypes)reader.GetInt32(3);
                    client.RefreshTokenLifeTime = reader.GetInt32(4);
                    client.AllowedOrigin = reader.GetString(5);
                    client.IsActive = reader.GetBoolean(6);
                }
            }
            else
            {
                client = null;
            }
            AuthDbConnection.bakserverWebBoardAuthConn.Close();
            return client;
        }
    }
}
