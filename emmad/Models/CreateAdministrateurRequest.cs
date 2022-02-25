using System.ComponentModel.DataAnnotations;

namespace emmad.Models
{
    public class CreateAdministrateurRequest
    {
        [Required]
        public string prenom { get; set; }
        [Required]
        public string nom { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
    }
}
