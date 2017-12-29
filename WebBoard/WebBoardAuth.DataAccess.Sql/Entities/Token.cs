using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebBoardAuth.DataAccess.Sql.Entities
{
    public class Token
    {
        [Required]
        public string Key { get; set; }
        
        [Required]
        public string Value { get; set; }
    }
}
