using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebBoardAuth.Api.Models.ExternalAccountViewModel
{
    public class ExternalLoginToRegisterViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string ReturnUrl { get; set; }

        public ExternalLoginToRegisterViewModel()
        {
            this.Email = null;
            this.Password = null;
            this.ConfirmPassword = null;
            this.ReturnUrl = null;
        }
    }
}
