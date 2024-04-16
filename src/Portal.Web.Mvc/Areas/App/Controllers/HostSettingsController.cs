using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Configuration;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using Portal.Authorization;
using Portal.Authorization.Users;
using Portal.Configuration;
using Portal.Configuration.Host;
using Portal.Editions;
using Portal.Timing;
using Portal.Timing.Dto;
using Portal.Web.Areas.App.Models.HostSettings;
using Portal.Web.Controllers;

namespace Portal.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Host_Settings)]
    public class HostSettingsController : PortalControllerBase
    {
        private readonly UserManager _userManager;
        private readonly IHostSettingsAppService _hostSettingsAppService;
        private readonly IEditionAppService _editionAppService;
        private readonly ITimingAppService _timingAppService;
        private readonly IAppConfigurationAccessor _configurationAccessor;

        public HostSettingsController(
            IHostSettingsAppService hostSettingsAppService,
            UserManager userManager,
            IEditionAppService editionAppService,
            ITimingAppService timingAppService,
            IAppConfigurationAccessor configurationAccessor)
        {
            _hostSettingsAppService = hostSettingsAppService;
            _userManager = userManager;
            _editionAppService = editionAppService;
            _timingAppService = timingAppService;
            _configurationAccessor = configurationAccessor;
        }

        public async Task<ActionResult> Index()
        {
            var hostSettings = await _hostSettingsAppService.GetAllSettings();
            var editionItems =
                await _editionAppService.GetEditionComboboxItems(hostSettings.TenantManagement.DefaultEditionId);
            var timezoneItems = await _timingAppService.GetTimezoneComboboxItems(new GetTimezoneComboboxItemsInput
            {
                DefaultTimezoneScope = SettingScopes.Application,
            });

            var user = await _userManager.GetUserAsync(AbpSession.ToUserIdentifier());

            ViewBag.CurrentUserEmail = user.EmailAddress;

            var model = new HostSettingsViewModel
            {
                Settings = hostSettings,
                EditionItems = editionItems,
                TimezoneItems = timezoneItems
            };

            AddEnabledSocialLogins(model);

            return View(model);
        }

        private void AddEnabledSocialLogins(HostSettingsViewModel model)
        {
            if (!bool.Parse(_configurationAccessor.Configuration["Authentication:AllowSocialLoginSettingsPerTenant"]))
            {
                return;
            }

            if (bool.Parse(_configurationAccessor.Configuration["Authentication:Facebook:IsEnabled"]))
            {
                model.EnabledSocialLoginSettings.Add("Facebook");
            }

            if (bool.Parse(_configurationAccessor.Configuration["Authentication:Google:IsEnabled"]))
            {
                model.EnabledSocialLoginSettings.Add("Google");
            }

            if (bool.Parse(_configurationAccessor.Configuration["Authentication:Twitter:IsEnabled"]))
            {
                model.EnabledSocialLoginSettings.Add("Twitter");
            }

            if (bool.Parse(_configurationAccessor.Configuration["Authentication:Microsoft:IsEnabled"]))
            {
                model.EnabledSocialLoginSettings.Add("Microsoft");
            }

            if (bool.Parse(_configurationAccessor.Configuration["Authentication:OpenId:IsEnabled"]))
            {
                model.EnabledSocialLoginSettings.Add("OpenId");
            }
            
            if (bool.Parse(_configurationAccessor.Configuration["Authentication:WsFederation:IsEnabled"]))
            {
                model.EnabledSocialLoginSettings.Add("WsFederation");
            }
        }
    }
}