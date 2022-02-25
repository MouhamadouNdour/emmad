using System;
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
        public int id_createur { get; set; }
        public DateTime date_created { get; set; }
        public byte[] password { get; set; }
        public byte[] salt { get; set; }
    }
}
