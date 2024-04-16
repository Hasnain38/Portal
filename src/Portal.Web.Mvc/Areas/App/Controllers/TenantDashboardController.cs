using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Authorization;
using Portal.DashboardCustomization;
using System.Threading.Tasks;
using Portal.Web.Areas.App.Startup;

namespace Portal.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Dashboard)]
    public class TenantDashboardController : CustomizableDashboardControllerBase
    {
        public TenantDashboardController(DashboardViewConfiguration dashboardViewConfiguration, 
            IDashboardCustomizationAppService dashboardCustomizationAppService) 
            : base(dashboardViewConfiguration, dashboardCustomizationAppService)
        {

        }

        public async Task<ActionResult> Index()
        {
            return await GetView(PortalDashboardCustomizationConsts.DashboardNames.DefaultTenantDashboard);
        }
    }
}