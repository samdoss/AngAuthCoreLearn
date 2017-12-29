namespace WebBoardWeb.Auth.Models
{
    public class AuthViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string GrantType { get; set; }
        public string Access { get; set; }
        public string Refresh { get; set; }
        public string Key { get; set; }
        public string CheckSum { get; set; }
        public string ClientId { get; set; }

        public AuthViewModel()
        {
            UserName = null;
            Password = null;
            GrantType = null;
            Access = null;
            Refresh = null;
            Key = null;
            CheckSum = null;
            ClientId = null;
        }
    }
}
