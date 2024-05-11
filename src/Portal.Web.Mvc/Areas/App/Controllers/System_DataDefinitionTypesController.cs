using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Web.Areas.App.Models.System_DataDefinitionTypes;
using Portal.Web.Controllers;
using Portal.Authorization;
using Portal.SystemDataDefinitionType;
using Portal.SystemDataDefinitionType.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

using Abp.BackgroundJobs;
using Portal.Storage;
using Portal.DataImporting.Excel;
using Portal.Web.Controllers;

namespace Portal.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_System_DataDefinitionTypes)]
    public class System_DataDefinitionTypesController : ExcelImportControllerBase
    {
        private readonly ISystem_DataDefinitionTypesAppService _system_DataDefinitionTypesAppService;

        protected readonly IBinaryObjectManager _binaryObjectManager;
        protected readonly IBackgroundJobManager _backgroundJobManager;

        public override string ImportExcelPermission => AppPermissions.Pages_System_DataDefinitionTypes_Create;

        public System_DataDefinitionTypesController(ISystem_DataDefinitionTypesAppService system_DataDefinitionTypesAppService, IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager) : base(binaryObjectManager, backgroundJobManager)
        {
            _system_DataDefinitionTypesAppService = system_DataDefinitionTypesAppService;

            _binaryObjectManager = binaryObjectManager;
            _backgroundJobManager = backgroundJobManager;

        }

        public ActionResult Index()
        {
            var model = new System_DataDefinitionTypesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_System_DataDefinitionTypes_Create, AppPermissions.Pages_System_DataDefinitionTypes_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetSystem_DataDefinitionTypeForEditOutput getSystem_DataDefinitionTypeForEditOutput;

            if (id.HasValue)
            {
                getSystem_DataDefinitionTypeForEditOutput = await _system_DataDefinitionTypesAppService.GetSystem_DataDefinitionTypeForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getSystem_DataDefinitionTypeForEditOutput = new GetSystem_DataDefinitionTypeForEditOutput
                {
                    System_DataDefinitionType = new CreateOrEditSystem_DataDefinitionTypeDto()
                };
            }

            var viewModel = new CreateOrEditSystem_DataDefinitionTypeModalViewModel()
            {
                System_DataDefinitionType = getSystem_DataDefinitionTypeForEditOutput.System_DataDefinitionType,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewSystem_DataDefinitionTypeModal(int id)
        {
            var getSystem_DataDefinitionTypeForViewDto = await _system_DataDefinitionTypesAppService.GetSystem_DataDefinitionTypeForView(id);

            var model = new System_DataDefinitionTypeViewModel()
            {
                System_DataDefinitionType = getSystem_DataDefinitionTypeForViewDto.System_DataDefinitionType
            };

            return PartialView("_ViewSystem_DataDefinitionTypeModal", model);
        }

        public override async Task EnqueueExcelImportJobAsync(ImportFromExcelJobArgs args)
        {
            await BackgroundJobManager.EnqueueAsync<ImportSystem_DataDefinitionTypesToExcelJob, ImportFromExcelJobArgs>(args);
        }

    }
}