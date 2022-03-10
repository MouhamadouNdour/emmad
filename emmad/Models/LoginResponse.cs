using System;

namespace emmad.Models
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string? Telephone { get; set; }
        public DateTime Date_created { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiration { get; set; }
    }
}
