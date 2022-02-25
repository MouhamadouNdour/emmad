using emmad.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;

namespace emmad.Helper
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<int> _roles;
        public AuthorizeAttribute(params int[] roles)
        {
            _roles = roles ?? new int[] { };
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var administrateur = (Administrateur)context.HttpContext.Items["Administrateur"];
            if (administrateur == null)
            {
                // Utilisateur n'est pas connecté ou le role ne correspond pas
                context.Result = new JsonResult(new { message = "Vous n'avez pas les droits nécessaires." }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
