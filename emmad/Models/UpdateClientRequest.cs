﻿using System;
namespace emmad.Models
{
    public class UpdateClientRequest
    {
        public string nom { get; set; }
        public string prenom { get; set; }
        public string email { get; set; }
        public string telephone { get; set; }
        public int age { get; set; }
    }
}
