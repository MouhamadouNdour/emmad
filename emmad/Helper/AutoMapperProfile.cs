using AutoMapper;
using emmad.Entity;
using emmad.Models;

namespace emmad.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            CreateMap<Administrateur, LoginResponse>();
            CreateMap<CreateAdministrateurRequest, Administrateur>();
            CreateMap<Administrateur, CreateResponse>();
            CreateMap<CreateOrganisationRequest, Organisation>();
            CreateMap<CreateClientRequest, Client>();
            CreateMap<Organisation, CreateOrganisationResponse>();
            CreateMap<Client, CreateClientResponse>();
        }
    }
}
