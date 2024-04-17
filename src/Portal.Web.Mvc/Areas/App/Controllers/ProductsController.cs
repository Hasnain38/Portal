using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Web.Areas.App.Models.Products;
using Portal.Web.Controllers;
using Portal.Authorization;
using Portal.ProductNamespeace;
using Portal.ProductNamespeace.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

using Abp.BackgroundJobs;
using Portal.Storage;
using Portal.DataImporting.Excel;
using Portal.Web.Controllers;

namespace Portal.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Products)]
    public class ProductsController : ExcelImportControllerBase
    {
        private readonly IProductsAppService _productsAppService;

        protected readonly IBinaryObjectManager _binaryObjectManager;
        protected readonly IBackgroundJobManager _backgroundJobManager;

        public override string ImportExcelPermission => AppPermissions.Pages_Products_Create;

        public ProductsController(IProductsAppService productsAppService, IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager) : base(binaryObjectManager, backgroundJobManager)
        {
            _productsAppService = productsAppService;

            _binaryObjectManager = binaryObjectManager;
            _backgroundJobManager = backgroundJobManager;

        }

        public ActionResult Index()
        {
            var model = new ProductsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Products_Create, AppPermissions.Pages_Products_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetProductForEditOutput getProductForEditOutput;

            if (id.HasValue)
            {
                getProductForEditOutput = await _productsAppService.GetProductForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getProductForEditOutput = new GetProductForEditOutput
                {
                    Product = new CreateOrEditProductDto()
                };
                getProductForEditOutput.Product.CreatedOnUtc = DateTime.Now;
                getProductForEditOutput.Product.UpdatedOnUtc = DateTime.Now;
            }

            var viewModel = new CreateOrEditProductModalViewModel()
            {
                Product = getProductForEditOutput.Product,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewProductModal(int id)
        {
            var getProductForViewDto = await _productsAppService.GetProductForView(id);

            var model = new ProductViewModel()
            {
                Product = getProductForViewDto.Product
            };

            return PartialView("_ViewProductModal", model);
        }

        public override async Task EnqueueExcelImportJobAsync(ImportFromExcelJobArgs args)
        {
            await BackgroundJobManager.EnqueueAsync<ImportProductsToExcelJob, ImportFromExcelJobArgs>(args);
        }

    }
}