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
            CreateMap<CreateClientRequest, Client>()
                .ForMember(c => c.Photos, opt => opt.Ignore());
            CreateMap<Organisation, CreateOrganisationResponse>();
            CreateMap<Client, CreateClientResponse>();
            CreateMap<CreateRdvRequest, Rdv>();
            CreateMap<Rdv, CreateRdvResponse>();
            CreateMap<Administrateur, AdministrateurResponse>();
            CreateMap<UpdateAdministrateurRequest, Administrateur>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        // Ignore les valeurs null et les champs vides
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));
            CreateMap<Organisation, OrganisationResponse>();
            CreateMap<UpdateOrganisationRequest, Organisation>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        // Ignore les valeurs null et les champs vides
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(int) && int.Parse(prop.ToString()) == 0) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));
        }
    }
}
