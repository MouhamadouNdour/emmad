using emmad.Context;
using emmad.Entity;
using emmad.Interface;
using emmad.Models;
using System;
using System.Linq;

namespace emmad.Services
{
    public class AdministrateurService : IAdministrateur
    {

        private readonly MasterContext MasterContext;


        public AdministrateurService(MasterContext masterContext)
        {
            MasterContext = masterContext;
        }

        public Administrateur CreateAdministrateur(CreateAdministrateurRequest model)
        {
            if (string.IsNullOrEmpty(model.email))
            {
                throw new Exception("Email: Vide");
            }
            if (string.IsNullOrEmpty(model.nom))
            {
                throw new Exception("Nom: Vide");
            }
            if (string.IsNullOrEmpty(model.prenom))
            {
                throw new Exception("Prenom: Vide");
            }

            var existingAdmin = MasterContext.administrateur
                            .FirstOrDefault(a => a.email == model.email);

            if(existingAdmin != null)
            {
                throw new Exception("L'adresse email "+ model.email + " est déjà utilisé.");
            }

            var admin = new Administrateur();
            admin.email = model.email;
            admin.nom = model.nom; 
            admin.prenom = model.prenom;

            MasterContext.administrateur.Add(admin);
            MasterContext.SaveChanges();

            return admin;
        }
    }
}
