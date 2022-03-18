using emmad.Entity;

namespace emmad.Models
{
    public class AdministrateurResponse
    {
        public int id { get; set; }
        public int id_createur { get; set; }
        public string prenom { get; set; }
        public string nom { get; set; }
        public string email { get; set; }
    }
}
