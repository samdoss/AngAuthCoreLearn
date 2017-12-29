using System;
using System.Collections.Generic;
using System.Text;

namespace WebBoardAuth.Logic.Models
{
    public class TokensForLoginDto
    {
        public byte[] Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }

        public TokensForLoginDto()
        {
            this.Secret = null;
            this.Issuer = null;
            this.Audience = null;
        }
    }
}
