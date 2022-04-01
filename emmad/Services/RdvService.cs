using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using emmad.Context;
using emmad.Entity;
using emmad.Interface;
using emmad.Models;
using emmad.Parameter;
using emmad.Settings;
using Microsoft.Extensions.Options;

namespace emmad.Services
{
    public class RdvService : IRdv
    {
        private readonly MasterContext MasterContext;
        private readonly IMapper Mapper;
        private readonly AppSettings appSettings;

        public RdvService(MasterContext masterContext, IMapper mapper, IOptions<AppSettings> settings)
        {
            MasterContext = masterContext;
            Mapper = mapper;
            appSettings = settings.Value;
        }


        public CreateRdvResponse CreateRdv(Administrateur administrateur, CreateRdvRequest model)
        {

            var client = MasterContext.client.Find(model.id_client);

            var organisation = MasterContext.organisation.Find(client.id_organisation);

            if (organisation.id_administrateur != administrateur.id)
            {
                throw new Exception("Vous n'avez pas les droits de créer ce rdv.");
            }

            if (string.IsNullOrWhiteSpace(model.resume))
            {
                throw new Exception("resume : Vide");
            }
            if (string.IsNullOrWhiteSpace(model.lieu))
            {
                throw new Exception("Adresse : Vide");
            }

            var dateRdv = DateTime.ParseExact(model.dateRdv, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (dateRdv < DateTime.MinValue)
            {
                throw new Exception("Date : Vide");
            }

            var existingDateForClient = MasterContext.rdv
                                                .FirstOrDefault(c => c.date == dateRdv
                                                && c.id_client == model.id_client);

            if (existingDateForClient != null)
            {
                throw new Exception("Un Rdv pour ce client à cette date existe déjà email.");
            }


            var rdv = Mapper.Map<Rdv>(model);
            rdv.id_client = model.id_client;
            rdv.date = dateRdv;

            MasterContext.rdv.Add(rdv);
            MasterContext.SaveChanges();

            return Mapper.Map<CreateRdvResponse>(rdv);

        }


        public void DeleteRdv(Administrateur connectedUser, int id)
        {
            var rdv = MasterContext.rdv.Find(id);

            if(rdv == null)
            {
                throw new Exception("Ce RDV n'existe pas.");
            }

            var client = MasterContext.client.Find(rdv.id_client);

            if(client == null)
            {
                throw new Exception("Ce client n'existe pas pour ce RDV.");
            }

            var organisation = MasterContext.organisation.Find(client.id_organisation);

            if (client == null)
            {
                throw new Exception("Ce client n'a pas d'organisation.");
            }

            if(organisation.id_administrateur != connectedUser.id)
            {
                throw new Exception("Vous n'avez pas les droits de supprimer ce rdv.");
            }           

            MasterContext.rdv.Remove(rdv);
            MasterContext.SaveChanges();
        }

        public IEnumerable GetRdv(Administrateur administrateur, int idClient, PageParameters pageParameters)
        {
            var rdvs = MasterContext.rdv
                         .Where(c => c.id_client == idClient)
                         .Skip((pageParameters.page - 1) * pageParameters.size)
                         .Take(pageParameters.size)
                         .ToList();

            List<Rdv> RdvsResponse = new List<Rdv>();

            foreach (var rdv in rdvs)
            {
                var RdvResponse = new Rdv()
                {
                    id = rdv.id,
                    resume = rdv.resume,
                    date = rdv.date,
                    id_client = rdv.id_client,
                    lieu = rdv.lieu
                };

                RdvsResponse.Add(RdvResponse);
            }

            return RdvsResponse;
        }

        public RdvResponse Update(Administrateur administrateur, int idRdv, UpdateRdvRequest model)
        {

            var rdv = MasterContext.rdv.Find(idRdv);

            if (rdv == null)
            {
                throw new Exception("Ce rdv n'existe pas.");
            }

            var client = MasterContext.client.Find(rdv.id_client);

            if (client == null)
            {
                throw new Exception("Ce client n'existe pas.");
            }

            var organisation = MasterContext.organisation.Find(client.id_organisation);

            if (organisation == null)
            {
                throw new Exception("Cette organisation n'existe pas.");
            }
            if (organisation.id_administrateur == administrateur.id)
            {
                // Copie du model au l'entite organisation
                Mapper.Map(model, rdv);
                if(!string.IsNullOrEmpty(model.dateRdv))
                {
                    rdv.date = DateTime.ParseExact(model.dateRdv, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                MasterContext.rdv.Update(rdv);
                MasterContext.SaveChanges();

                return Mapper.Map<RdvResponse>(rdv);
            }
            else
            {
                throw new KeyNotFoundException("Vous n'avez pas les droits de modifier ce rdv.");
            }
        }
    }
}
