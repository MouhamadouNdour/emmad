using System.ComponentModel.DataAnnotations;

namespace emmad.Entity
{
    public class Photo
    {
        [Key]
        public int id { get; set; }
        public string lien { get; set; }
        public int id_client { get; set; }
        public Client Client { get; set; }
    }
}
