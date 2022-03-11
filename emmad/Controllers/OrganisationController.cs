using emmad.Helper;
using emmad.Interface;
using emmad.Models;
using Microsoft.AspNetCore.Mvc;
using System;
namespace emmad.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrganisationController : BaseController
    {
        private IOrganisation Service;

        public OrganisationController(IOrganisation _service)
        {
            Service = _service;
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateOrganisation(CreateOrganisationRequest model)
        {
            try
            {
                return Ok(new
                {
                    data = Service.CreateOrganisation(Administrateur, model),
                    message = "Organisation créée avec succès."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpGet]
        public IActionResult GetOrganisation()
        {
            try
            {
                return Ok(Service.GetOrganisation(Administrateur));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }


    }
}
