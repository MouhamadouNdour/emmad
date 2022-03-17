using System;
using System.Globalization;
using System.Linq;
using AutoMapper;
using emmad.Context;
using emmad.Entity;
using emmad.Interface;
using emmad.Models;
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
            

            var rdv = Mapper.Map<Rdv>(model);
            rdv.id_client = model.id_client;
            rdv.date = dateRdv;

            MasterContext.rdv.Add(rdv);
            MasterContext.SaveChanges();

            return Mapper.Map<CreateRdvResponse>(rdv);

        }
    }
}
