using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebBoardAuth.Authorization
{
    public interface IRefreshToken
    {
        string GenerateNewToken(string access, string refresh);
        Redis Redis_obj { get; set; }

    }
    public class RefreshToken : IRefreshToken
    {

        private string authServ;
        private string redisServ;
        //private string token_detail ;
        private Redis redis_obj;
        public RefreshToken()
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
        public RefreshToken(string auth_serv, string redis_serv)
        {
            authServ = auth_serv;
            redisServ = redis_serv;
            //token_detail = "";
            if (checkDebug())
            {
                redis_obj = new Redis(auth_serv);
            }
            else
            {
                redis_obj = new Redis(redisServ);
            }

        }
        public Redis Redis_obj
        {
            get
            {
                return redis_obj;
            }
            set
            {
                this.redis_obj = value;
            }
        }
        public string GenerateNewToken(string access, string refresh)
        {
            string token_detail = "";

            if (Redis_obj.IsExistToken(access, refresh))
            {
                var data = "";
                var t = Task.Run(async () => { data = await GenerateNewTokenAsyc(access, refresh); });
                t.Wait();

                if (data != "error" && data != "")
                {
                    token_detail = SetupToken(data);
                }
                else
                {
                    token_detail = "error";
                }


            }
            return token_detail;

        }
        public async Task<string> GenerateNewTokenAsyc(string access, string refresh)
        {
            string path = "api/token";
            string data = "";
            IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string,string>("access",access),
                        new KeyValuePair<string,string>("refresh",refresh),
                        new KeyValuePair<string,string>("grant_type","refresh")

                    };
            HttpContent param = new FormUrlEncodedContent(queries);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");

                using (HttpResponseMessage response = await client.PostAsync(authServ + path, param))
                {
                    using (HttpContent content = response.Content)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            data = response.Content.ReadAsStringAsync().Result;

                        }
                        else
                        {
                            data = "error";
                        }

                    }
                }

            }
            return data;
        }//end asysc

        public string SetupToken(string response)
        {
            var tk = "";
            try
            {

                var obj = JsonConvert.DeserializeObject<JObject>(response);
                var a = "";
                var r = "";
                foreach (var t in obj)
                {
                    if (t.Key == "access_token") a = t.Value.ToString();
                    if (t.Key == "refresh_token") r = t.Value.ToString();
                }

                tk = "a_token=" + a + "&r_token=" + r;
            }
            catch (Exception e)
            {
                tk = "error";
            }
            return tk;
        }


    }
}
