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

        public ClientController(IClient _service)
        {
            Service = _service;
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateClient(CreateClientRequest model)
        {
            try
            {
                return Ok(new
                {
                    data = Service.CreateClient(Administrateur, model),
                    message = "Client créé avec succès."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpDelete("{idOrganisation:int}/{idClient:int}")]
        [Authorize]
        public IActionResult DeleteClient(int idOrganisation, int idClient)
        {
            try
            {
                Service.DeleteClient(Administrateur, idOrganisation, idClient);

                return Ok(new
                {
                    message = "Client supprimé avec succès."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{idOrganisation:int}")]
        [Authorize]
        public IActionResult GetClient(int idOrganisation, [FromQuery] PageParameters pageParameters)
        {
            try
            {
                return Ok(Service.GetClient(Administrateur, idOrganisation, pageParameters));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

    }
}
