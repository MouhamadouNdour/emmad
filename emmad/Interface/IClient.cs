using System.Collections;
using emmad.Entity;
using emmad.Models;
using emmad.Parameter;

namespace emmad.Interface
{
    public interface IClient
    {
        public CreateClientResponse CreateClient(Administrateur administrateur, CreateClientRequest model);
        public IEnumerable GetClient(Administrateur administrateur, int idOrganisation, PageParameters pageParameters);
        public void DeleteClient(Administrateur administrateur,int idOrganisation, int idClient);
    }
}
