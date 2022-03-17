using System.Collections;
using emmad.Entity;
using emmad.Models;
using emmad.Parameter;

namespace emmad.Interface
{
    public interface IOrganisation
    {
        public CreateOrganisationResponse CreateOrganisation(Administrateur administrateur, CreateOrganisationRequest model);
        public IEnumerable GetOrganisation(Administrateur administrateur, OrganisationParameters organisationParameters);
        public void DeleteOrganisation(Administrateur administrateur, int idOrganisation);


    }
}
   

