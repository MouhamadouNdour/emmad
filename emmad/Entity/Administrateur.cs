using System.ComponentModel.DataAnnotations;

namespace emmad.Entity
{
    public class Administrateur
    {
        [Key]
        public int id { get; set; }
        public string prenom { get; set; }
        public string nom { get; set; }
        public string email { get; set; }
    }
}
