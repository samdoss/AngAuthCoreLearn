using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebBoardAuth.Api.Models.AccountModel
{
    public class ChangePasswordViewModel
    {
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public ChangePasswordViewModel()
        {
            this.UserName = null;
            this.OldPassword = null;
            this.NewPassword = null;
            this.ConfirmPassword = null;
        }
    }
}
