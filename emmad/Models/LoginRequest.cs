using System.ComponentModel.DataAnnotations;

namespace emmad.Models
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        public string passe { get; set; }
    }
}
