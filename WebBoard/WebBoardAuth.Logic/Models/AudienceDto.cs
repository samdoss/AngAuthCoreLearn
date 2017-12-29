using System.ComponentModel.DataAnnotations;

namespace WebBoardAuth.Logic.Models
{
    public class AudienceDto
    {
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        public string Issuer { get; set; }
    }
}
