﻿using Microsoft.AspNetCore.Mvc;
using Portal.DashboardCustomization;
using Portal.DashboardCustomization.Dto;
using Portal.Web.Areas.App.Models.CustomizableDashboard;
using Portal.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using Portal.Web.Areas.App.Startup;

namespace Portal.Web.Areas.App.Controllers
{
    public abstract class CustomizableDashboardControllerBase : PortalControllerBase
    {
        protected readonly IDashboardCustomizationAppService DashboardCustomizationAppService;
        protected readonly DashboardViewConfiguration DashboardViewConfiguration;

        protected CustomizableDashboardControllerBase(
            DashboardViewConfiguration dashboardViewConfiguration,
            IDashboardCustomizationAppService dashboardCustomizationAppService)
        {
            DashboardViewConfiguration = dashboardViewConfiguration;
            DashboardCustomizationAppService = dashboardCustomizationAppService;
        }

        public async Task<PartialViewResult> AddWidgetModal(string dashboardName, string pageId)
        {
            var availableWidgets = await DashboardCustomizationAppService.GetAllAvailableWidgetDefinitionsForPage(
                new GetAvailableWidgetDefinitionsForPageInput()
                {
                    Application = PortalDashboardCustomizationConsts.Applications.Mvc,
                    PageId = pageId,
                    DashboardName = dashboardName
                });

            var viewModel = new AddWidgetViewModel
            {
                Widgets = availableWidgets,
                DashboardName = dashboardName,
                PageId = pageId
            };

            return PartialView(
                "~/Areas/App/Views/Shared/Components/CustomizableDashboard/_AddWidgetModal.cshtml",
                viewModel
            );
        }

        protected async Task<ActionResult> GetView(string dashboardName)
        {
            var dashboardDefinition = DashboardCustomizationAppService.GetDashboardDefinition(
                new GetDashboardInput
                {
                    DashboardName = dashboardName,
                    Application = PortalDashboardCustomizationConsts.Applications.Mvc
                }
            );

            var userDashboard = await DashboardCustomizationAppService.GetUserDashboard(new GetDashboardInput
                {
                    DashboardName = dashboardName,
                    Application = PortalDashboardCustomizationConsts.Applications.Mvc
                }
            );

            // Show only view defined widgets
            foreach (var userDashboardPage in userDashboard.Pages)
            {
                userDashboardPage.Widgets = userDashboardPage.Widgets
                    .Where(w => DashboardViewConfiguration.WidgetViewDefinitions.ContainsKey(w.WidgetId)).ToList();
            }

            dashboardDefinition.Widgets = dashboardDefinition.Widgets.Where(dw =>
                userDashboard.Pages.Any(p => p.Widgets.Select(w => w.WidgetId).Contains(dw.Id))).ToList();

            return View("~/Areas/App/Views/Shared/Components/CustomizableDashboard/Index.cshtml",
                new CustomizableDashboardViewModel(
                    dashboardDefinition,
                    userDashboard)
            );
        }
    }
}
