using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Portal.OrderNamespeace.Dtos;
using Portal.Dto;

namespace Portal.OrderNamespeace
{
    public interface IOrdersAppService : IApplicationService
    {
        Task<PagedResultDto<GetOrderForViewDto>> GetAll(GetAllOrdersInput input);

        Task<GetOrderForViewDto> GetOrderForView(int id);

        Task<GetOrderForEditOutput> GetOrderForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditOrderDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetOrdersToExcel(GetAllOrdersForExcelInput input);

    }
}