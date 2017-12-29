using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebBoardAuth.Api.Models.AccountModel
{
    public class ActivateViewModel
    {
        public string Username { get; set; }
        public string Token { get; set; }

        public ActivateViewModel()
        {
            this.Username = null;
            this.Token = null;
        }
    }
}
