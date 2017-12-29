using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebBoardAuth.Api.Models.AccountModel
{
    public class SetLockViewModel
    {
        public string UserName { get; set; }
        public double Minutes { get; set; }

        public SetLockViewModel()
        {
            this.UserName = null;
            this.Minutes = 0.0;
        }
    }
}
