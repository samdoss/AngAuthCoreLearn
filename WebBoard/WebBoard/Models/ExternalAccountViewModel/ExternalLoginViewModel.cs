using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebBoardAuth.Api.Models.ExternalAccountViewModel
{
    public class ExternalLoginViewModel
    {
        public string Provider { get; set; }
        public string ReturnUrl { get; set; }

        public ExternalLoginViewModel()
        {
            this.Provider = null;
            this.ReturnUrl = null;
        }
    }
}
