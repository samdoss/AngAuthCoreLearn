using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBoardAuth.Api.Models.AccountModel
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public LoginViewModel(string username, string password, bool remember = false)
        {
            this.Email = username;
            this.Password = password;
            this.RememberMe = remember;
        }
    }
}
