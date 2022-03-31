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
        public IActionResult GetOrganisation([FromQuery] PageParameters pageParameters)
        {
            try
            {
                return Ok(Service.GetOrganisation(Administrateur, pageParameters));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpDelete("{idOrganisation:int}")]
        [Authorize]
        public IActionResult DeleteOrganisation(int idOrganisation)
        {
            try
            {
                Service.DeleteOrganisation(Administrateur, idOrganisation);

                return Ok(new
                {
                    message = "Organisation supprimé avec succès."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{idOrganisation:int}")]
        [Authorize]
        public IActionResult Update(int idOrganisation, UpdateOrganisationRequest model)
        {
            try
            {
                return Ok(new
                {
                    data = Service.Update(Administrateur, idOrganisation, model),
                    message = "Organisation modifiée avec succès."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }

}

