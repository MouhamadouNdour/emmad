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
        private readonly IMapper Mapper;
        private readonly AppSettings appSettings;
        private readonly IEmail EmailService;

        public AdministrateurService(MasterContext masterContext, IMapper mapper, IOptions<AppSettings> settings, IEmail emailService)
        {
            MasterContext = masterContext;
            Mapper = mapper;
            appSettings = settings.Value;
            EmailService = emailService;
        }

        public LoginResponse Login(LoginRequest model)
        {
            // Assertion des données
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Passe))
            {
                throw new Exception("Adresse email ou mot de passe invalide.");
            }

            // Vérification de l'adresse email dans la base de données
            var administrateur = MasterContext.administrateur.SingleOrDefault(u => u.email == model.Email);

            if (administrateur == null)
            {
                throw new Exception("Erreur sur l'adresse email ou le mot de passe.");
            }

            // Verification du mot de passe
            if (!SecurityService.VerifyPasswordHash(administrateur.password, administrateur.salt, model.Passe))
            {
                throw new Exception("Erreur sur l'adresse email ou le mot de passe.");
            }

            // Connexion reussie
            var response = Mapper.Map<LoginResponse>(administrateur);
            DateTime tokenExpiration;

            response.Token = SecurityService.GenerateJwtToken(administrateur, appSettings, out tokenExpiration);
            response.TokenExpiration = tokenExpiration;

            return response;
        }

        public CreateResponse CreateAdministrateur(Administrateur connectedUser, CreateAdministrateurRequest model)
        {
            DateTime? date_crea = null;
            if(connectedUser != null)
            {
                date_crea = MasterContext.administrateur
                    .OrderByDescending(a => a.date_created)
                    .Where(a => a.id_createur == connectedUser.id)
                    .Select(a => a.date_created)
                    .FirstOrDefault();
            }

            if (date_crea != null && ((DateTime)date_crea).AddMinutes(1) > DateTime.Now)
            {
                throw new Exception("Veuillez attendre 1 minute avant de pouvoir créer un utilisateur");
            }
            if (string.IsNullOrEmpty(model.Email))
            {
                throw new Exception("Email: Vide");
            }
            if (string.IsNullOrEmpty(model.Nom))
            {
                throw new Exception("Nom: Vide");
            }
            if (string.IsNullOrEmpty(model.Prenom))
            {
                throw new Exception("Prenom: Vide");
            }

            var existingAdmin = MasterContext.administrateur
                            .FirstOrDefault(a => a.email == model.Email);

            if(existingAdmin != null)
            {
                throw new Exception("L'adresse email "+ model.Email + " est déjà utilisé.");
            }

            if (string.IsNullOrWhiteSpace(model.Passe))
            {
                throw new Exception("Veuillez saisir un mot de passe.");
            }

            var administrateur = Mapper.Map<Administrateur>(model);
            administrateur.date_created = DateTime.Now;

            byte[] passwordHash, passwordSalt;
            SecurityService.CreatePasswordHash(model.Passe, out passwordHash, out passwordSalt);
            administrateur.password = passwordHash;
            administrateur.salt = passwordSalt;

            administrateur.id_createur = connectedUser.id;

            SendEmailVerification(administrateur);

            MasterContext.administrateur.Add(administrateur);
            MasterContext.SaveChanges();

            return Mapper.Map<CreateResponse>(administrateur);
        }

        private void SendEmailVerification(Administrateur admin)
        {
            string message = $@"<p>Commencez dès maintenant à utiliser notre API</p>
                             <p>Email : {admin.email}</p>";

            EmailService.Send(
                to: admin.email,
                subject: "[Emmad Application] - Nouvelle inscription dans notre application",
                html: $@"<h4>Bienvenue</h4>
                         <p> Voici le lien vers la doc d'utilisation de l'API : </p>
                         <a href='https://localhost:5001'>Documentation de l'API</a>
                         <p>Merci pour votre inscription !</p>
                         {message}"
            );
        }
    }
}
