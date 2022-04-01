using emmad.Entity;
using emmad.Helper;
using emmad.Interface;
using emmad.Models;
using emmad.Parameter;
using Microsoft.AspNetCore.Mvc;
using System;
namespace emmad.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrganisationController : BaseController
    {
        private IOrganisation Service;
        private readonly ILoggerService _logger;
        private string accessController = "Accès à OrganisationController : ";

        public OrganisationController(IOrganisation _service, ILoggerService logger)
        {
            Service = _service;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateOrganisation(CreateOrganisationRequest model)
        {
            _logger.LogInfo(accessController + "Tentative de création d'une organisation.");
            try
            {
                _logger.LogDebug(HttpContext.Request.Method + " Request - " + HttpContext.Request.Host + " => " + HttpContext.Response.StatusCode.ToString());
                return Ok(new
                {
                    data = Service.CreateOrganisation(Administrateur, model),
                    message = "Organisation créée avec succès."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetOrganisation([FromQuery] PageParameters pageParameters)
        {
            _logger.LogInfo(accessController + "Tentative de récupération des organisations.");
            try
            {
                _logger.LogDebug(HttpContext.Request.Method + " Request - " + HttpContext.Request.Host + " => " + HttpContext.Response.StatusCode.ToString());
                return Ok(Service.GetOrganisation(Administrateur, pageParameters));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{idOrganisation:int}")]
        [Authorize]
        public IActionResult DeleteOrganisation(int idOrganisation)
        {
            _logger.LogInfo(accessController + "Tentative de suppression d'une organisation.");
            try
            {
                _logger.LogDebug(HttpContext.Request.Method + " Request - " + HttpContext.Request.Host + " => " + HttpContext.Response.StatusCode.ToString());
                Service.DeleteOrganisation(Administrateur, idOrganisation);
                _logger.LogWarn("Suprression avec succès.");

                return Ok(new
                {
                    message = "Organisation supprimé avec succès."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{idOrganisation:int}")]
        [Authorize]
        public IActionResult Update(int idOrganisation, UpdateOrganisationRequest model)
        {
            _logger.LogInfo(accessController + "Tentative de mise à jour des données d'une organisation.");
            try
            {
                _logger.LogDebug(HttpContext.Request.Method + " Request - " + HttpContext.Request.Host + " => " + HttpContext.Response.StatusCode.ToString());
                _logger.LogWarn("Mise à jour des infos avec succès.");

                return Ok(new
                {
                    data = Service.Update(Administrateur, idOrganisation, model),
                    message = "Organisation modifiée avec succès."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

    }

}

