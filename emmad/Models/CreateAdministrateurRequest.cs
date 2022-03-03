using System;
using System.ComponentModel.DataAnnotations;

namespace emmad.Models
{
    public class CreateAdministrateurRequest
    {
        [Required]
        public string Prenom { get; set; }
        [Required]
        public string Nom { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Passe { get; set; }

    }
}

