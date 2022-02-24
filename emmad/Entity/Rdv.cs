using System;
using System.ComponentModel.DataAnnotations;

namespace emmad.Entity
{
    public class Rdv
    {
        [Key]
        public int id { get; set; }
        public string resume { get; set; }
        public DateTime date { get; set; }
        public int id_client { get; set; }
        public Client Client { get; set; }
    }
}
