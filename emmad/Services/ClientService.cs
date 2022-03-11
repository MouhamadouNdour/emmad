using AutoMapper;
using emmad.Context;
using emmad.Entity;
using emmad.Interface;
using emmad.Models;
using emmad.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Linq;

namespace emmad.Services
{
    public class ClientService : IClient
    {

        private readonly MasterContext MasterContext;
        private readonly IMapper Mapper;
        private readonly AppSettings appSettings;

        public ClientService(MasterContext masterContext, IMapper mapper, IOptions<AppSettings> settings)
        {
            MasterContext = masterContext;
            Mapper = mapper;
            appSettings = settings.Value;
        }

        public CreateClientResponse CreateClient(Administrateur administrateur, CreateClientRequest model)
        {
            if (string.IsNullOrWhiteSpace(model.nom))
            {
                throw new Exception("Nom : Vide");
            }
            if (string.IsNullOrWhiteSpace(model.prenom))
            {
                throw new Exception("Prenom : Vide");
            }
            if (string.IsNullOrWhiteSpace(model.email))
            {
                throw new Exception("Email : Vide");
            }
            if (string.IsNullOrWhiteSpace(model.telephone))
            {
                throw new Exception("Email : Vide");
            }

            var existingOrganisation = MasterContext.organisation
                            .FirstOrDefault(c => c.id == model.societe);


            if (existingOrganisation == null)
            {
                throw new Exception("L'organisation n'existe pas.");
            }

            var existingClient = MasterContext.client
                            .FirstOrDefault(c => c.email == model.email
                                                && c.id_organisation == model.societe);

            if (existingClient != null)
            {
                throw new Exception("L'adresse email du client " + model.email + " est déjà utilisé dans cette organisation.");
            }

            var client = Mapper.Map<Client>(model);
            client.id_organisation = model.societe;

            MasterContext.client.Add(client);
            MasterContext.SaveChanges();

            return Mapper.Map<CreateClientResponse>(client);
        }

        public IEnumerable GetClient(Administrateur administrateur, int idOrganisation )
        {
            var ListClient  = MasterContext.client
                         .Where(i => i.id_organisation == idOrganisation)
                         .ToList();

            return ListClient;

        }

    }
}
