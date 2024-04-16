using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Web.Controllers;

namespace Portal.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize]
    public class WelcomeController : PortalControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}