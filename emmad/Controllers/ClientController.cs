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
    public class ClientController : BaseController
    {

        private IClient Service;
        private readonly ILoggerService _logger;
        private string accessController = "Accès à ClientController : ";

        public ClientController(IClient _service, ILoggerService logger)
        {
            Service = _service;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateClient(CreateClientRequest model)
        {
            _logger.LogInfo(accessController + "Tentative de création d'un client.");
            try
            {
                var client = Service.CreateClient(Administrateur, model);

                _logger.LogDebug(HttpContext.Request.Method + " Request - " + HttpContext.Request.Host + " => " + HttpContext.Response.StatusCode.ToString());
                return Ok(new
                {
                    data = client,
                    message = "Client créé avec succès."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpDelete("{idOrganisation:int}/{idClient:int}")]
        [Authorize]
        public IActionResult DeleteClient(int idOrganisation, int idClient)
        {
            _logger.LogInfo(accessController + "Tentative de suppression d'un client.");
            try
            {
                Service.DeleteClient(Administrateur, idOrganisation, idClient);
                _logger.LogDebug(HttpContext.Request.Method + " Request - " + HttpContext.Request.Host + " => " + HttpContext.Response.StatusCode.ToString());
                _logger.LogWarn("Suprression avec succès.");
                return Ok(new
                {
                    message = "Client supprimé avec succès."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{idOrganisation:int}")]
        [Authorize]
        public IActionResult GetClient(int idOrganisation, [FromQuery] PageParameters pageParameters)
        {
            _logger.LogInfo(accessController + "Tentative de récupération d'un client.");
            try
            {
                var client = Service.GetClient(Administrateur, idOrganisation, pageParameters);

                _logger.LogDebug(HttpContext.Request.Method + " Request - " + HttpContext.Request.Host + " => " + HttpContext.Response.StatusCode.ToString());
                return Ok(client);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }

        }


        [HttpPut("{idClient:int}")]
        [Authorize]
        public IActionResult Update(int idClient, UpdateClientRequest model)
        {
            _logger.LogInfo(accessController + "Tentative de mise à jour des données d'un client.");
            try
            {
                var client = Service.Update(Administrateur, idClient, model);

                _logger.LogDebug(HttpContext.Request.Method + " Request - " + HttpContext.Request.Host + " => " + HttpContext.Response.StatusCode.ToString());
                _logger.LogWarn("Mise à jour des infos avec succès.");
                return Ok(new
                {
                    data = client,
                    message = "Client modifiée avec succès."
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
