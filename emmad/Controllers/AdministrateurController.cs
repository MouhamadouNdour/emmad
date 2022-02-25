using emmad.Context;
using emmad.Entity;
using emmad.Interface;
using emmad.Models;
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

        private IAdministrateur Service;

        public AdministrateurController(IAdministrateur _service)
        {
            Service = _service;
        }

        [HttpPost]
        public IActionResult CreateAdministrateur(CreateAdministrateurRequest model)
        {
            try
            {
                Service.CreateAdministrateur(model);
                return Ok("Adminstrateur créé avec succès.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
