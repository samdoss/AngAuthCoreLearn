using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebBoardAuth.Api.Models.AccountModel
{
    public class PreResetPasswordViewModel
    {
        public string UserName { get; set; }
        public string FromUrl { get; set; }

        public PreResetPasswordViewModel()
        {
            this.UserName = null;
            this.FromUrl = null;
        }
    }
}
