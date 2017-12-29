using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebBoardAuth.Api.Models
{
    public class ValidationViewModel
    {
        public string Token { get; set; }
        public string ClientSecret { get; set; }
        public string RoleRequire { get; set; }

        public ValidationViewModel()
        {
            this.Token = null;
            this.ClientSecret = null;
            this.RoleRequire = null;
        }
    }


}