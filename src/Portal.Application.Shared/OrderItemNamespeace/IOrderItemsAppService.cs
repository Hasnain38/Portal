using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Portal.OrderItemNamespeace.Dtos;
using Portal.Dto;

namespace Portal.OrderItemNamespeace
{
    public interface IOrderItemsAppService : IApplicationService
    {
        Task<PagedResultDto<GetOrderItemForViewDto>> GetAll(GetAllOrderItemsInput input);

        Task<GetOrderItemForViewDto> GetOrderItemForView(int id);

        Task<GetOrderItemForEditOutput> GetOrderItemForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditOrderItemDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetOrderItemsToExcel(GetAllOrderItemsForExcelInput input);

    }
}