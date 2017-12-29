using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebBoardAuth.Api.Models
{
    public class TokensViewModel
    {
        public string Access { get; set; }
        public string Refresh { get; set; }

        public TokensViewModel()
        {
            this.Access = null;
            this.Refresh = null;
        }
    }


}