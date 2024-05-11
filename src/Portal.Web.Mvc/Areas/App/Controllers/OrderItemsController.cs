using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Web.Areas.App.Models.OrderItems;
using Portal.Web.Controllers;
using Portal.Authorization;
using Portal.OrderItemNamespeace;
using Portal.OrderItemNamespeace.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

using Abp.BackgroundJobs;
using Portal.Storage;
using Portal.DataImporting.Excel;
using Portal.Web.Controllers;

namespace Portal.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_OrderItems)]
    public class OrderItemsController : ExcelImportControllerBase
    {
        private readonly IOrderItemsAppService _orderItemsAppService;

        protected readonly IBinaryObjectManager _binaryObjectManager;
        protected readonly IBackgroundJobManager _backgroundJobManager;

        public override string ImportExcelPermission => AppPermissions.Pages_OrderItems_Create;

        public OrderItemsController(IOrderItemsAppService orderItemsAppService, IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager) : base(binaryObjectManager, backgroundJobManager)
        {
            _orderItemsAppService = orderItemsAppService;

            _binaryObjectManager = binaryObjectManager;
            _backgroundJobManager = backgroundJobManager;

        }

        public ActionResult Index()
        {
            var model = new OrderItemsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_OrderItems_Create, AppPermissions.Pages_OrderItems_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetOrderItemForEditOutput getOrderItemForEditOutput;

            if (id.HasValue)
            {
                getOrderItemForEditOutput = await _orderItemsAppService.GetOrderItemForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getOrderItemForEditOutput = new GetOrderItemForEditOutput
                {
                    OrderItem = new CreateOrEditOrderItemDto()
                };
                getOrderItemForEditOutput.OrderItem.CreatedOnUtc = DateTime.Now;
                getOrderItemForEditOutput.OrderItem.UpdatedOnUtc = DateTime.Now;
            }

            var viewModel = new CreateOrEditOrderItemModalViewModel()
            {
                OrderItem = getOrderItemForEditOutput.OrderItem,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewOrderItemModal(int id)
        {
            var getOrderItemForViewDto = await _orderItemsAppService.GetOrderItemForView(id);

            var model = new OrderItemViewModel()
            {
                OrderItem = getOrderItemForViewDto.OrderItem
            };

            return PartialView("_ViewOrderItemModal", model);
        }

        public override async Task EnqueueExcelImportJobAsync(ImportFromExcelJobArgs args)
        {
            await BackgroundJobManager.EnqueueAsync<ImportOrderItemsToExcelJob, ImportFromExcelJobArgs>(args);
        }

    }
}