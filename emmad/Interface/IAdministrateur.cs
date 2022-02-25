using emmad.Entity;
using emmad.Models;

namespace emmad.Interface
{
    public interface IAdministrateur
    {
        public Administrateur CreateAdministrateur(CreateAdministrateurRequest model);
    }
}
