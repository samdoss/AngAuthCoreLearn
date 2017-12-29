using WebBoardAuth.DataAccess.Sql;
using WebBoardAuth.DataAccess.Sql.Entities;
using WebBoardAuth.Logic.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Data;
using System.Data.SqlClient;
using WebBoardAuth.Logic.Service.Interface;
using System.Threading.Tasks;


namespace WebBoardAuth.Logic.Service.Implement
{
    public class AudienceService : IAudienceService
    {
        AuthDbConnection AuthDbConnection;

        public static ConcurrentDictionary<string, Audience> AudiencesList = new ConcurrentDictionary<string, Audience>();

        public AudienceService(AuthDbConnection _conn)
        {
            AuthDbConnection = _conn;
        }

        public async Task<Audience> AddAudience(AudienceDto doc)
        {
            var clientId = Guid.NewGuid().ToString("N");
            var key = new byte[32];
            RandomNumberGenerator.Create().GetBytes(key);
            var base64Secret = Convert.ToBase64String(key);
            var newAudience = new Audience()
            {
                ClientId = clientId,
                Base64Secret = base64Secret,
                Name = doc.Name,
                Issuer = doc.Issuer
            };

            await AuthDbConnection.bakserverWebBoardAuthConn.OpenAsync();

            string sql = "INSERT INTO Audiences(ClientId,Base64Secret,Name,Issuer) VALUES(@param1,@param2,@param3,@param4)";
            SqlCommand cmd = new SqlCommand(sql, AuthDbConnection.bakserverWebBoardAuthConn);
            cmd.Parameters.Add("@param1", SqlDbType.VarChar, 32).Value = newAudience.ClientId;
            cmd.Parameters.Add("@param2", SqlDbType.VarChar, 80).Value = newAudience.Base64Secret;
            cmd.Parameters.Add("@param3", SqlDbType.VarChar, 100).Value = newAudience.Name;
            cmd.Parameters.Add("@param4", SqlDbType.VarChar, 100).Value = newAudience.Issuer;
            cmd.CommandType = CommandType.Text;

            await cmd.ExecuteNonQueryAsync();
            AuthDbConnection.bakserverWebBoardAuthConn.Close();

            return newAudience;
        }

        public async Task<Audience> FindAudience(string clientId)
        {
            Audience audience = new Audience();

            await AuthDbConnection.bakserverWebBoardAuthConn.OpenAsync();

            string sql = "SELECT * FROM Audiences WHERE ClientId ='" + clientId + "'";
            SqlCommand cmd = new SqlCommand(sql, AuthDbConnection.bakserverWebBoardAuthConn);
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    audience.ClientId = reader.GetString(0);
                    audience.Base64Secret = reader.GetString(1);
                    audience.Name = reader.GetString(2);
                    audience.Issuer = reader.GetString(3);
                }
            }
            else
            {
                audience = null;
            }
            AuthDbConnection.bakserverWebBoardAuthConn.Close();

            return audience;
        }

        public async Task<List<Audience>> GetAllAudience()
        {
            string sql = "SELECT * FROM Audiences";
            using (var command = new SqlCommand(sql, AuthDbConnection.bakserverWebBoardAuthConn))
            {
                await AuthDbConnection.bakserverWebBoardAuthConn.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var list = new List<Audience>();
                    while (await reader.ReadAsync())
                        list.Add(new Audience
                        {
                            ClientId = reader.GetString(0),
                            Base64Secret = reader.GetString(1),
                            Name = reader.GetString(2),
                            Issuer = reader.GetString(3)
                        });
                    AuthDbConnection.bakserverWebBoardAuthConn.Close();
                    return list;
                }
            }
        }
    }
}
