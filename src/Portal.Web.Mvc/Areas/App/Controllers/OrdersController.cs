using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Web.Areas.App.Models.Orders;
using Portal.Web.Controllers;
using Portal.Authorization;
using Portal.OrderNamespeace;
using Portal.OrderNamespeace.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

using Abp.BackgroundJobs;
using Portal.Storage;
using Portal.DataImporting.Excel;
using Portal.Web.Controllers;

namespace Portal.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Orders)]
    public class OrdersController : ExcelImportControllerBase
    {
        private readonly IOrdersAppService _ordersAppService;

        protected readonly IBinaryObjectManager _binaryObjectManager;
        protected readonly IBackgroundJobManager _backgroundJobManager;

        public override string ImportExcelPermission => AppPermissions.Pages_Orders_Create;

        public OrdersController(IOrdersAppService ordersAppService, IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager) : base(binaryObjectManager, backgroundJobManager)
        {
            _ordersAppService = ordersAppService;

            _binaryObjectManager = binaryObjectManager;
            _backgroundJobManager = backgroundJobManager;

        }

        public ActionResult Index()
        {
            var model = new OrdersViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Orders_Create, AppPermissions.Pages_Orders_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetOrderForEditOutput getOrderForEditOutput;

            if (id.HasValue)
            {
                getOrderForEditOutput = await _ordersAppService.GetOrderForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getOrderForEditOutput = new GetOrderForEditOutput
                {
                    Order = new CreateOrEditOrderDto()
                };
                getOrderForEditOutput.Order.CreatedOnUtc = DateTime.Now;
                getOrderForEditOutput.Order.UpdatedOnUtc = DateTime.Now;
            }

            var viewModel = new CreateOrEditOrderModalViewModel()
            {
                Order = getOrderForEditOutput.Order,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewOrderModal(int id)
        {
            var getOrderForViewDto = await _ordersAppService.GetOrderForView(id);

            var model = new OrderViewModel()
            {
                Order = getOrderForViewDto.Order
            };

            return PartialView("_ViewOrderModal", model);
        }

        public override async Task EnqueueExcelImportJobAsync(ImportFromExcelJobArgs args)
        {
            await BackgroundJobManager.EnqueueAsync<ImportOrdersToExcelJob, ImportFromExcelJobArgs>(args);
        }

    }
}