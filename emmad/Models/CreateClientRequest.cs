using System.ComponentModel.DataAnnotations;

namespace emmad.Models
{
    public class CreateClientRequest
    {
        [Required]
        public string nom { get; set; }
        [Required]
        public string prenom { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        [StringLength(10)]
        public string telephone { get; set; }
        public int age { get; set; }
        public int societe { get; set; }
    }
}
