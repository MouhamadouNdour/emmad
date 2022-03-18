﻿using System;
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

        [HttpDelete("{idRdv:int}")]
        [Authorize]
        public IActionResult DeleteRdv(int idRdv)
        {
            try
            {
                Service.DeleteRdv(Administrateur, idRdv);

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
            try
            {
                return Ok(Service.GetRdv(Administrateur, idClient, pageParameters));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
    }
}
