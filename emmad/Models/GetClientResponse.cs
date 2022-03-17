using emmad.Entity;
using System.Collections.Generic;

namespace emmad.Models
{
    public class GetClientResponse
    {
        public int id { get; set; }
        public int id_organisation { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string email { get; set; }
        public string telephone { get; set; }
        public int? age { get; set; }
        public Organisation Organisation { get; set; }
        public List<PhotoResponse> Photos { get; set; }
    }
}
