using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Portal.Web.Areas.App.Models.Layout;
using Portal.Web.Session;
using Portal.Web.Views;

namespace Portal.Web.Areas.App.Views.Shared.Themes.Default.Components.AppDefaultBrand
{
    public class AppDefaultBrandViewComponent : PortalViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppDefaultBrandViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headerModel = new HeaderViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(headerModel);
        }
    }
}
