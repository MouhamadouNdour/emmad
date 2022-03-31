using System;
namespace emmad.Models
{
    public class OrganisationResponse
    {
        public int id { get; set; }
        public string nom { get; set; }
        public string adresse { get; set; }
        public int id_administrateur { get; set; }
        public int nb_salarie { get; set; }
        public string logo { get; set; }
    }
}
