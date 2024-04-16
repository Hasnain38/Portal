using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Portal.Web.Areas.App.Models.Layout;
using Portal.Web.Views;

namespace Portal.Web.Areas.App.Views.Shared.Components.AppChatToggler
{
    public class AppChatTogglerViewComponent : PortalViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string cssClass, string iconClass = "flaticon-chat-2 fs-4")
        {
            return Task.FromResult<IViewComponentResult>(View(new ChatTogglerViewModel
            {
                CssClass = cssClass,
                IconClass = iconClass
            }));
        }
    }
}
