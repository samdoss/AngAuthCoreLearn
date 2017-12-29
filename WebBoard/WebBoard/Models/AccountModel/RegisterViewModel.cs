using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebBoardAuth.Api.Models.AccountModel
{
    public class RegisterViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public RegisterViewModel()
        {
            this.UserName = null;
            this.Password = null;
            this.ConfirmPassword = null;
        }
    }
}
