using emmad.Context;
using emmad.Entity;
using emmad.Helper;
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
    public class AdministrateurController : BaseController
    {

        private IAdministrateur Service;

        public AdministrateurController(IAdministrateur _service)
        {
            Service = _service;
        }

        
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest Model)
        {
            try
            {
                var response = Service.Login(Model);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }

        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateAdministrateur(CreateAdministrateurRequest model)
        {
            try
            {
                return Ok(new {
                    data = Service.CreateAdministrateur(Administrateur, model),
                    message = "Administrateur créé avec succès."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
