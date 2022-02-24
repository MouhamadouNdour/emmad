using emmad.Context;
using emmad.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace emmad.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdministrateurController : ControllerBase
    {
       

        private readonly MasterContext MasterContext;

        public AdministrateurController(MasterContext masterContext)
        {
            MasterContext = masterContext;
        }


        [HttpGet("administrateurs")]
        public IEnumerable<Administrateur> GetAdministrateur()
        {
            var adminstrateurs = MasterContext.administrateur.ToList();
            return adminstrateurs;                                   
        }


        [HttpGet("organisations")]
        public IEnumerable<Organisation> GetOrganisation()
        {
            var organisations = MasterContext.organisation
                                .Include(o => o.Administrateur)
                                .ToList();
            return organisations;
        }
    }
}
