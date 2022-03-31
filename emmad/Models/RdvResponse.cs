using System;
namespace emmad.Models
{
    public class RdvResponse
    {
        public int id { get; set; }
        public string resume { get; set; }
        public DateTime date { get; set; }
        public int id_client { get; set; }
        public string lieu { get; set; }

    }
}
