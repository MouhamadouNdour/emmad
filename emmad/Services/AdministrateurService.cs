using AutoMapper;
using emmad.Context;
using emmad.Entity;
using emmad.Interface;
using emmad.Models;
using emmad.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace emmad.Services
{
    public class AdministrateurService : IAdministrateur
    {

        private readonly MasterContext MasterContext;
        private readonly EmailService EmailService;
        private readonly IMapper Mapper;
        private readonly AppSettings appSettings;

        public AdministrateurService(MasterContext masterContext, EmailService emailService, IMapper mapper, IOptions<AppSettings> settings)
        {
            MasterContext = masterContext;
            EmailService = emailService;
            Mapper = mapper;
            appSettings = settings.Value;
        }

        public LoginResponse Login(LoginRequest model)
        {
            // Assertion des données
            if (string.IsNullOrEmpty(model.email) || string.IsNullOrEmpty(model.passe))
            {
                throw new Exception("Adresse email ou mot de passe invalide.");
            }

            // Vérification de l'adresse email dans la base de données
            var administrateur = MasterContext.administrateur.SingleOrDefault(u => u.email == model.email);

            if (administrateur == null)
            {
                throw new Exception("Erreur sur l'adresse email ou le mot de passe.");
            }

            // Verification du mot de passe
            if (!SecurityService.VerifyPasswordHash(administrateur.password, administrateur.salt, model.passe))
            {
                throw new Exception("Erreur sur l'adresse email ou le mot de passe.");
            }

            // Connexion reussie
            var response = Mapper.Map<LoginResponse>(administrateur);

            response.token = SecurityService.GenerateJwtToken(administrateur, appSettings);

            return response;
        }

        public Administrateur CreateAdministrateur(CreateAdministrateurRequest model)
        {


            var id_connect = 1;
            var date_crea = MasterContext.administrateur
                .OrderByDescending(a => a.date_created)
                .Where(a => a.id_createur == id_connect)
                .Select( a => a.date_created)
                .FirstOrDefault();

            if(date_crea.AddMinutes(1) < DateTime.Now)
            {
                throw new Exception("Veuillez attendre 1 minute avant de pouvoir créer un utilisateur");
            }
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

            if (string.IsNullOrWhiteSpace(model.passe))
            {
                throw new Exception("Veuillez saisir un mot de passe.");
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
