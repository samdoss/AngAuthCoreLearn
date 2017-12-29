using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebBoardAuth.Authorization
{
    public interface IRedis
    {
        ConnectionMultiplexer Connection { get; set; }
        IDatabase Cache { get; set; }
        Task<string> getSecret(string token, string client_secret, string role);
        bool IsExistToken(string access, string refresh);
    }
    public class Redis : IRedis
    {
        private static string RedisServ;
        private IDatabase cache;
        private ConnectionMultiplexer connection;
        public Redis()
        {

        }
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
        public Redis(string serv)
        {
            RedisServ = serv;
            if (!checkDebug())
            {
                Connection = lazyConnection.Value;
                cache = Connection.GetDatabase();
            }

        }
        public IDatabase Cache
        {
            get
            {
                return cache;
            }
            set
            {
                this.cache = value;
            }
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(RedisServ);
        });

        public ConnectionMultiplexer Connection
        {
            get
            {
                return connection;
            }
            set
            {
                this.connection = value;
            }
        }


        public async Task<string> getSecret(string token, string client_secret, string role)
        {
            //connect to redis 
            try
            {
                /*
                 * get secret from redis
                 * key => token 
                 * value => secret
                */
                string secret = "";
                if (checkDebug())
                {
                    secret = await getTokenSecret(token, client_secret, role);
                }
                else
                {
                    secret = cache.StringGet(token);
                }

                return secret;
            }
            catch (Exception e)
            {
                return "token_error";
            }

        }
        public bool IsExistToken(string access, string refresh)
        {
            try
            {
                bool acce = false;
                bool refr = false;

                if (checkDebug())
                {
                    acce = CheckExistToken(access);
                    refr = CheckExistToken(refresh);
                }
                else
                {
                    acce = cache.KeyExists(access);
                    refr = cache.KeyExists(refresh);
                }

                return acce && refr;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //Api get from Redis 
        private string secret_token = null;
        public async Task<string> getTokenSecret(string token, string client_secret, string role)
        {
            return await AsyncGetTokenSecret(token, client_secret, role);
        }

        async Task<string> AsyncGetTokenSecret(string token, string client_secret, string role)
        {
            var path = "api/Validation/CheckAccessibility";
            var keyValue = new Dictionary<string, string>
            {
                {"Token",token},
                {"ClientSecret",client_secret},
                {"RoleRequire",role}
            };
            var json = JsonConvert.SerializeObject(keyValue);
            HttpClient httpClient = new HttpClient();
            var str_content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                using (HttpResponseMessage response = await client.PostAsync(RedisServ + path, str_content))
                {
                    using (HttpContent content = response.Content)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            try
                            {
                                return response.Content.ReadAsStringAsync().Result;
                            }
                            catch (Exception e)
                            {
                                return null;
                            }
                        }
                        return null;
                    }
                }
            }
        }

        //CheckIsExistToken
        private bool result_api = false;
        public bool CheckExistToken(string key)
        {
            var t = Task.Run(async () => { await AsyncCheckExistToken(key); });
            t.Wait();
            return result_api;
        }

        async Task AsyncCheckExistToken(string key)
        {
            var path = "api/Validation/IsExists";
            var json = JsonConvert.SerializeObject(key);
            HttpClient httpClient = new HttpClient();
            var str_content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                using (HttpResponseMessage response = await client.PostAsync(RedisServ + path, str_content))
                {
                    using (HttpContent content = response.Content)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            try
                            {
                                result_api = true; // ExistToken
                            }

                            catch (Exception e)
                            {
                                result_api = false;
                            }
                        }
                    }
                }
            }
        }

        //public static string GenerateChecksum(string key)
        //{
        //    string secretPhase = "eXVpY3JlYXRlcmFpbnN0b3Jtd2hlbnNoZXJ1bg==";

        //    SHA256 sha256 = SHA256.Create();
        //    var bytes = Encoding.UTF8.GetBytes(string.Format("{0}{1}", key, secretPhase));
        //    var hash = sha256.ComputeHash(bytes);
        //    return BitConverter.ToString(hash);
        //}



    }
}
