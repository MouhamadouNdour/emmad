using System;
using emmad.Entity;
using emmad.Models;

namespace emmad.Interface
{
    public interface IRdv
    {
        public CreateRdvResponse CreateRdv(Administrateur administrateur, CreateRdvRequest model);
        public void DeleteRdv(Administrateur administrateur, int idRdv, int idClient);

    }
}

