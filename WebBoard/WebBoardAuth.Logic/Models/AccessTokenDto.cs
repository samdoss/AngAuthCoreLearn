using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebBoardAuth.Logic.Models
{
    public class AccessTokenDto
    {
        public DateTime now { get; set; }
        public string Sub { get; set; }
        public string Jti { get; set; }
        public string Iat { get; set; }
        public byte[] Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public SigningCredentials hashSecret { get; set; }
        public string Roles { get; set; }

        public AccessTokenDto()
        {
            this.now = DateTime.UtcNow;
            this.Sub = null;
            this.Jti = null;
            this.Iat = null;
            this.Secret = null;
            this.Issuer = null;
            this.Audience = null;
            this.hashSecret = null;
            this.Roles = "";
        }
    }
}
