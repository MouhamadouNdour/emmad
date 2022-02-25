using emmad.Entity;
using emmad.Models;

namespace emmad.Interface
{
    public interface IOrganisation
    {
        public CreateOrganisationResponse CreateOrganisation(Administrateur administrateur, CreateOrganisationRequest model);
        
    }
}

