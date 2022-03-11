using System.Collections;
using emmad.Entity;
using emmad.Models;

namespace emmad.Interface
{
    public interface IClient
    {
        public CreateClientResponse CreateClient(Administrateur administrateur, CreateClientRequest model);
        public IEnumerable GetClient(Administrateur administrateur, int idOrganisation);
        public void DeleteClient(Administrateur administrateur,int idOrganisation, int idClient);
    }
}
