using System.ComponentModel.DataAnnotations;

namespace Finshark.Dtos.account
{
    public class LoginDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
