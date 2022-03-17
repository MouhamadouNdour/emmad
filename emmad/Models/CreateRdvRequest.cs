using System;
using System.ComponentModel.DataAnnotations;

namespace emmad.Models
{
    public class CreateRdvRequest
    {
        [Required]
        public string resume { get; set; }
        [Required]
        public string dateRdv { get; set; }
        [Required]
        public string lieu { get; set; }

        public int id_client { get; set; }
    }
}
