using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebBoardAuth.Api.Models.AccountModel
{
    public class ResetPasswordViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }

        public ResetPasswordViewModel()
        {
            this.Email = null;
            this.Token = null;
            this.Password = null;
            this.ConfirmPassword = null;
        }
    }
}
