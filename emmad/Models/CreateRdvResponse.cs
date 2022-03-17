using System;
using System.ComponentModel.DataAnnotations;

namespace emmad.Models
{
    public class CreateRdvResponse
    {
        public int id { get; set; }
        public string resume { get; set; }
        public DateTime date { get; set; }
        public string lieu { get; set; }
        public int id_client { get; set; }
    }
}
