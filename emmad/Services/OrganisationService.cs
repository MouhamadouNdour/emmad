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
    public class OrganisationService : IOrganisation
    {
        private readonly MasterContext MasterContext;
        private readonly IMapper Mapper;
        private readonly AppSettings appSettings;

        public OrganisationService(MasterContext masterContext, IMapper mapper, IOptions<AppSettings> settings)
        {
            MasterContext = masterContext;
            Mapper = mapper;
            appSettings = settings.Value;
        }

        public CreateOrganisationResponse CreateOrganisation(Administrateur administrateur, CreateOrganisationRequest model)
        {
            if (string.IsNullOrWhiteSpace(model.nom))
            {
                throw new Exception("Nom : Vide");
            }
            if (string.IsNullOrWhiteSpace(model.adresse))
            {
                throw new Exception("Adresse : Vide");
            }
            if (model.nb_salarie <= 0)
            {
                throw new Exception("Salarié : Doit être supérieur à 0");
            }

            var existingOrganisation = MasterContext.organisation
                                        .FirstOrDefault(o => o.nom == model.nom);

            if(existingOrganisation != null)
            {
                throw new Exception("L'organisation " + model.nom + " existe déjà.");
            }

            var organisation = Mapper.Map<Organisation>(model);
            organisation.id_administrateur = administrateur.id;

            MasterContext.organisation.Add(organisation);
            MasterContext.SaveChanges();

            return Mapper.Map<CreateOrganisationResponse>(organisation);

        }
    }
}
