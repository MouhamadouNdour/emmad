using emmad.Entity;
using Microsoft.AspNetCore.Mvc;

namespace emmad.Controllers
{
    public class BaseController : ControllerBase
    {
        public new Administrateur Administrateur => (Administrateur)HttpContext.Items["Administrateur"];
    }
}
