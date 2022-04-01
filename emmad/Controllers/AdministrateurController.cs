using emmad.Context;
using emmad.Entity;
using emmad.Helper;
using emmad.Interface;
using emmad.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace emmad.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdministrateurController : BaseController
    {
        private IAdministrateur Service;
        private readonly ILoggerService _logger;

        public AdministrateurController(IAdministrateur _service, ILoggerService logger)
        {
            Service = _service;
            _logger = logger;
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest Model)
        {
            _logger.LogInfo("Accès à AdministrateurController : " + "Tentative de connexion d'un administrateur.");
            try
            {
                var response = Service.Login(Model);
                _logger.LogDebug(HttpContext.Request.Method + " Request - " + HttpContext.Request.Host + " => " +  HttpContext.Response.StatusCode.ToString());
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateAdministrateur(CreateAdministrateurRequest model)
        {
            _logger.LogInfo("Accès à AdministrateurController : " + "Tentative de création d'un administrateur.");
            try
            {
                _logger.LogDebug(HttpContext.Request.Method + " Request - " + HttpContext.Request.Host + " => " + HttpContext.Response.StatusCode.ToString());
                return Ok(new {
                    data = Service.CreateAdministrateur(Administrateur, model),
                    message = "Administrateur créé avec succès."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public IActionResult DeleteAdministrateur(int id)
        {
            _logger.LogInfo("Accès à AdministrateurController : " + "Tentative de suppression d'un administrateur.");
            try
            {
                if (id != Administrateur.id)
                {
                    _logger.LogWarn("Pas de droits nécessaires pour l'utilisateur [" + Administrateur.id+"]");
                    return Unauthorized(new { message = "Vous n'avez pas les droits nécessaires" });
                }

                Service.DeleteAdministrateur(id);

                _logger.LogDebug(HttpContext.Request.Method + " Request - " + HttpContext.Request.Host + " => " + HttpContext.Response.StatusCode.ToString());
                _logger.LogWarn("Suprression avec succès.");
                return Ok(new
                {
                    message = "Administrateur supprimé avec succès."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public IActionResult Update(int id, UpdateAdministrateurRequest model)
        {
            _logger.LogInfo("Accès à AdministrateurController : " + "Tentative de mise à jour des informations d'un administrateur.");
            try
            {
                if (id != Administrateur.id)
                {
                    _logger.LogWarn("Pas de droits nécessaires pour l'utilisateur [" + Administrateur.id + "]");
                    return Unauthorized(new { message = "Vous n'avez pas les droits nécessaires pour modifier cet administrateur." });
                }

                var admin = Service.Update(id, model);

                _logger.LogDebug(HttpContext.Request.Method + " Request - " + HttpContext.Request.Host + " => " + HttpContext.Response.StatusCode.ToString());
                _logger.LogWarn("Mise à jour des infos avec succès.");
                return Ok(new { data = admin, message = "Administrateur modifié avec succès." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
