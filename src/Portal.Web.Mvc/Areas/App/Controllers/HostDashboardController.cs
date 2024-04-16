using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Authorization;
using Portal.DashboardCustomization;
using System.Threading.Tasks;
using Portal.Web.Areas.App.Startup;

namespace Portal.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Host_Dashboard)]
    public class HostDashboardController : CustomizableDashboardControllerBase
    {
        public HostDashboardController(
            DashboardViewConfiguration dashboardViewConfiguration,
            IDashboardCustomizationAppService dashboardCustomizationAppService)
            : base(dashboardViewConfiguration, dashboardCustomizationAppService)
        {

        }

        public async Task<ActionResult> Index()
        {
            return await GetView(PortalDashboardCustomizationConsts.DashboardNames.DefaultHostDashboard);
        }
    }
}