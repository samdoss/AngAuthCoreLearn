using System;
using System.Collections.Generic;
using System.Text;

namespace WebBoardAuth.Logic.Models
{
    public class TokensForRefreshDto
    {
        public string access_token { get; set; }
        public string identity { get; set; }

        public TokensForRefreshDto()
        {
            this.access_token = null;
            this.identity = null;
        }
    }
}
