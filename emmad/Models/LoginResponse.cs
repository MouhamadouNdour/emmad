using System;

namespace emmad.Models
{
    public class LoginResponse
    {
        public int id { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string email { get; set; }
        public string? telephone { get; set; }
        public DateTime created_at { get; set; }
        public string token { get; set; }
    }
}
