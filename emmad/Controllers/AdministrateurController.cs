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
        private readonly ILogger _logger;

        public AdministrateurController(IAdministrateur _service, ILoggerFactory logFactory)
        {
            Service = _service;
            _logger = logFactory.CreateLogger<AdministrateurController>();
        }

        
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest Model)
        {
            _logger.LogInformation("Log message in the Login method");

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
            _logger.LogInformation("Log message in the Create method");
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

        [HttpDelete("{id:int}")]
        [Authorize]
        public IActionResult DeleteAdministrateur(int id)
        {
            _logger.LogInformation("Log message in the Delete method");
            try
            {
                if (id != Administrateur.id)
                {
                    return Unauthorized(new { message = "Unauthorized" });
                }

                Service.DeleteAdministrateur(id);

                return Ok(new
                {
                    message = "Administrateur supprimé avec succès."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
