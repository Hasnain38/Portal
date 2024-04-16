using Microsoft.AspNetCore.Mvc;
using Portal.Web.Controllers;

namespace Portal.Web.Public.Controllers
{
    public class AboutController : PortalControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}