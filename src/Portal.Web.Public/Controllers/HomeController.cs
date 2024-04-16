using Microsoft.AspNetCore.Mvc;
using Portal.Web.Controllers;

namespace Portal.Web.Public.Controllers
{
    public class HomeController : PortalControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}