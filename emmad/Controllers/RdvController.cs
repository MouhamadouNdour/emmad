using System;
using emmad.Helper;
using emmad.Interface;
using emmad.Models;
using Microsoft.AspNetCore.Mvc;

namespace emmad.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RdvController : BaseController
    {
        private IRdv Service;

        public RdvController(IRdv _service)
        {
            Service = _service;
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateRdv(CreateRdvRequest model)
        {
            try
            {
                return Ok(new
                {
                    data = Service.CreateRdv(Administrateur, model),
                    message = "Rdv créée avec succès."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
    }
}
