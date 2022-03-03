using System.ComponentModel.DataAnnotations;

namespace emmad.Models
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Passe { get; set; }
    }
}
