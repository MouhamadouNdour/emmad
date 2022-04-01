using System;
using emmad.Helper;
using emmad.Interface;
using emmad.Models;
using emmad.Parameter;
using Microsoft.AspNetCore.Mvc;

namespace emmad.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RdvController : BaseController
    {
        private IRdv Service;
        private readonly ILoggerService _logger;
        private string accessController = "Accès à RdvController : ";

        public RdvController(IRdv _service, ILoggerService logger)
        {
            Service = _service;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateRdv(CreateRdvRequest model)
        {
            _logger.LogInfo(accessController + "Tentative de création d'un rendez-vous.");
            try
            {
                var rdv = Service.CreateRdv(Administrateur, model);

                _logger.LogDebug(HttpContext.Request.Method + " Request - " + HttpContext.Request.Host + " => " + HttpContext.Response.StatusCode.ToString());
                return Ok(new
                {
                    data = rdv,
                    message = "Rdv créée avec succès."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpDelete("{idRdv:int}")]
        [Authorize]
        public IActionResult DeleteRdv(int idRdv)
        {
            _logger.LogInfo(accessController + "Tentative de suppression d'un rendez-vous.");
            try
            {
                Service.DeleteRdv(Administrateur, idRdv);
                _logger.LogDebug(HttpContext.Request.Method + " Request - " + HttpContext.Request.Host + " => " + HttpContext.Response.StatusCode.ToString());
                _logger.LogWarn("Suprression avec succès.");

                return Ok(new
                {
                    message = "Rdv supprimé avec succès."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{idClient:int}")]
        [Authorize]
        public IActionResult GetClient(int idClient, [FromQuery] PageParameters pageParameters)
        {
            _logger.LogInfo(accessController + "Tentative de récupération d'un rendez-vous.");
            try
            {
                var rdv = Service.GetRdv(Administrateur, idClient, pageParameters);

                _logger.LogDebug(HttpContext.Request.Method + " Request - " + HttpContext.Request.Host + " => " + HttpContext.Response.StatusCode.ToString());
                return Ok(rdv);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpPut("{idRdv:int}")]
        [Authorize]
        public IActionResult Update(int idRdv, UpdateRdvRequest model)
        {
            _logger.LogInfo(accessController + "Tentative de mise à jour des données d'un rendez-vous.");
            try
            {
                var rdv = Service.Update(Administrateur, idRdv, model);

                _logger.LogDebug(HttpContext.Request.Method + " Request - " + HttpContext.Request.Host + " => " + HttpContext.Response.StatusCode.ToString());
                _logger.LogWarn("Mise à jour des infos avec succès.");
                return Ok(new
                {
                    data = rdv,
                    message = "Rdv modifié avec succès."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
