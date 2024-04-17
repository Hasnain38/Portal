using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Web.Areas.App.Models.TestEntities;
using Portal.Web.Controllers;
using Portal.Authorization;
using Portal.TestEntityNamespeace;
using Portal.TestEntityNamespeace.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

using Abp.BackgroundJobs;
using Portal.Storage;
using Portal.DataImporting.Excel;
using Portal.Web.Controllers;

namespace Portal.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_TestEntities)]
    public class TestEntitiesController : ExcelImportControllerBase
    {
        private readonly ITestEntitiesAppService _testEntitiesAppService;

        protected readonly IBinaryObjectManager _binaryObjectManager;
        protected readonly IBackgroundJobManager _backgroundJobManager;

        public override string ImportExcelPermission => AppPermissions.Pages_TestEntities_Create;

        public TestEntitiesController(ITestEntitiesAppService testEntitiesAppService, IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager) : base(binaryObjectManager, backgroundJobManager)
        {
            _testEntitiesAppService = testEntitiesAppService;

            _binaryObjectManager = binaryObjectManager;
            _backgroundJobManager = backgroundJobManager;

        }

        public ActionResult Index()
        {
            var model = new TestEntitiesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_TestEntities_Create, AppPermissions.Pages_TestEntities_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetTestEntityForEditOutput getTestEntityForEditOutput;

            if (id.HasValue)
            {
                getTestEntityForEditOutput = await _testEntitiesAppService.GetTestEntityForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getTestEntityForEditOutput = new GetTestEntityForEditOutput
                {
                    TestEntity = new CreateOrEditTestEntityDto()
                };
            }

            var viewModel = new CreateOrEditTestEntityModalViewModel()
            {
                TestEntity = getTestEntityForEditOutput.TestEntity,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewTestEntityModal(int id)
        {
            var getTestEntityForViewDto = await _testEntitiesAppService.GetTestEntityForView(id);

            var model = new TestEntityViewModel()
            {
                TestEntity = getTestEntityForViewDto.TestEntity
            };

            return PartialView("_ViewTestEntityModal", model);
        }

        public override async Task EnqueueExcelImportJobAsync(ImportFromExcelJobArgs args)
        {
            await BackgroundJobManager.EnqueueAsync<ImportTestEntitiesToExcelJob, ImportFromExcelJobArgs>(args);
        }

    }
}