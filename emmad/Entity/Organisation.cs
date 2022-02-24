using System;
using System.ComponentModel.DataAnnotations;

namespace emmad.Entity
{
    public class Organisation
    {
        [Key]
        public int id { get; set; }
        public string adresse { get; set; }
        public int id_administrateur { get; set; }
        public Administrateur Administrateur { get; set; }
    }
}
