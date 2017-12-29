using System.ComponentModel.DataAnnotations;

namespace WebBoardAuth.Logic.Models
{
    public class ClientDto
    {
        [MaxLength(200)]
        [Required]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
