using AutoMapper;
using emmad.Entity;
using emmad.Models;

namespace emmad.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            CreateMap<Administrateur, LoginResponse>();
        }
    }
}
