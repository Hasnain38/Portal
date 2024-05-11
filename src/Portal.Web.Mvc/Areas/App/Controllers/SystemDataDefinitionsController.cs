using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Web.Areas.App.Models.SystemDataDefinitions;
using Portal.Web.Controllers;
using Portal.Authorization;
using Portal.System_DataDefinition;
using Portal.System_DataDefinition.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

using Abp.BackgroundJobs;
using Portal.Storage;
using Portal.DataImporting.Excel;
using Portal.Web.Controllers;

namespace Portal.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_SystemDataDefinitions)]
    public class SystemDataDefinitionsController : ExcelImportControllerBase
    {
        private readonly ISystemDataDefinitionsAppService _systemDataDefinitionsAppService;

        protected readonly IBinaryObjectManager _binaryObjectManager;
        protected readonly IBackgroundJobManager _backgroundJobManager;

        public override string ImportExcelPermission => AppPermissions.Pages_SystemDataDefinitions_Create;

        public SystemDataDefinitionsController(ISystemDataDefinitionsAppService systemDataDefinitionsAppService, IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager) : base(binaryObjectManager, backgroundJobManager)
        {
            _systemDataDefinitionsAppService = systemDataDefinitionsAppService;

            _binaryObjectManager = binaryObjectManager;
            _backgroundJobManager = backgroundJobManager;

        }

        public ActionResult Index()
        {
            var model = new SystemDataDefinitionsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_SystemDataDefinitions_Create, AppPermissions.Pages_SystemDataDefinitions_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetSystemDataDefinitionForEditOutput getSystemDataDefinitionForEditOutput;

            if (id.HasValue)
            {
                getSystemDataDefinitionForEditOutput = await _systemDataDefinitionsAppService.GetSystemDataDefinitionForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getSystemDataDefinitionForEditOutput = new GetSystemDataDefinitionForEditOutput
                {
                    SystemDataDefinition = new CreateOrEditSystemDataDefinitionDto()
                };
            }

            var viewModel = new CreateOrEditSystemDataDefinitionModalViewModel()
            {
                SystemDataDefinition = getSystemDataDefinitionForEditOutput.SystemDataDefinition,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewSystemDataDefinitionModal(int id)
        {
            var getSystemDataDefinitionForViewDto = await _systemDataDefinitionsAppService.GetSystemDataDefinitionForView(id);

            var model = new SystemDataDefinitionViewModel()
            {
                SystemDataDefinition = getSystemDataDefinitionForViewDto.SystemDataDefinition
            };

            return PartialView("_ViewSystemDataDefinitionModal", model);
        }

        public override async Task EnqueueExcelImportJobAsync(ImportFromExcelJobArgs args)
        {
            await BackgroundJobManager.EnqueueAsync<ImportSystemDataDefinitionsToExcelJob, ImportFromExcelJobArgs>(args);
        }

    }
}