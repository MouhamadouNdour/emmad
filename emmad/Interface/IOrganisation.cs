using System.Collections;
using emmad.Entity;
using emmad.Models;

namespace emmad.Interface
{
    public interface IOrganisation
    {
        public CreateOrganisationResponse CreateOrganisation(Administrateur administrateur, CreateOrganisationRequest model);
        public IEnumerable GetOrganisation(Administrateur administrateur);
        public void DeleteOrganisation(Administrateur administrateur, int idOrganisation);


    }
}
   
