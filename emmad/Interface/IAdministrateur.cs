using emmad.Entity;
using emmad.Models;

namespace emmad.Interface
{
    public interface IAdministrateur
    {
        public CreateResponse CreateAdministrateur(Administrateur administrateur, CreateAdministrateurRequest model);
        public LoginResponse Login(LoginRequest model);
        public void DeleteAdministrateur(int id);
        public AdministrateurResponse Update(int id, UpdateAdministrateurRequest model);
    }
}
