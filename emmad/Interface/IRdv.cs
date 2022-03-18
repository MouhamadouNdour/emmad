using System;
using System.Collections;
using emmad.Entity;
using emmad.Models;
using emmad.Parameter;

namespace emmad.Interface
{
    public interface IRdv
    {
        public CreateRdvResponse CreateRdv(Administrateur administrateur, CreateRdvRequest model);
        public IEnumerable GetRdv(Administrateur administrateur, int idOrganisation, PageParameters pageParameters);
        public void DeleteRdv(Administrateur administrateur, int idRdv);

    }
}

