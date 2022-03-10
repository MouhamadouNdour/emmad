using System.ComponentModel.DataAnnotations;

namespace emmad.Models
{
    public class CreateOrganisationRequest
    {
        [Required]
        public string nom { get; set; }
        [Required]
        public string adresse { get; set; }
        [Required]
        public int nb_salarie { get; set; }
    }
}
