using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Portal.Web.Areas.App.Models.Layout;
using Portal.Web.Views;

namespace Portal.Web.Areas.App.Views.Shared.Components.AppToggleDarkMode
{
    public class AppToggleDarkModeViewComponent : PortalViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string cssClass, bool isDarkModeActive)
        {
            return Task.FromResult<IViewComponentResult>(View(new ToggleDarkModeViewModel(cssClass, isDarkModeActive)));
        }
    }
}