using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Portal.Common.Dto;
using Portal.Editions.Dto;

namespace Portal.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false);

        Task<PagedResultDto<FindUsersOutputDto>> FindUsers(FindUsersInput input);

        GetDefaultEditionNameOutput GetDefaultEditionName();
    }
}