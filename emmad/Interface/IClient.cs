using emmad.Entity;
using emmad.Models;

namespace emmad.Interface
{
    public interface IClient
    {
        public CreateClientResponse CreateClient(Administrateur administrateur, CreateClientRequest model);
    }
}
