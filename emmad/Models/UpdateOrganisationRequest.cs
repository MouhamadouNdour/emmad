using System;
namespace emmad.Models
{
    public class UpdateOrganisationRequest
    {
        public string nom { get; set; }
        public string adresse { get; set; }
        public int nb_salarie { get; set; }
        public string? logo { get; set; }
    }
}
