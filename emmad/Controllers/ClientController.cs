using emmad.Helper;
using emmad.Interface;
using emmad.Models;
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

    }
}
