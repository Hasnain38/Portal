using Portal.Core.Dependency;
using Portal.Mobile.MAUI.Services.UI;

namespace Portal.Mobile.MAUI.Shared
{
    public class PortalMainLayoutPageComponentBase : PortalComponentBase
    {
        protected PageHeaderService PageHeaderService { get; set; }

        protected DomManipulatorService DomManipulatorService { get; set; }

        public PortalMainLayoutPageComponentBase()
        {
            PageHeaderService = DependencyResolver.Resolve<PageHeaderService>();
            DomManipulatorService = DependencyResolver.Resolve<DomManipulatorService>();
        }

        protected async Task SetPageHeader(string title)
        {
            PageHeaderService.Title = title;
            PageHeaderService.ClearButton();
            await DomManipulatorService.ClearModalBackdrop(JS);
        }

        protected async Task SetPageHeader(string title, List<PageHeaderButton> buttons)
        {
            PageHeaderService.Title = title;
            PageHeaderService.SetButtons(buttons);
            await DomManipulatorService.ClearModalBackdrop(JS);
        }
    }
}
