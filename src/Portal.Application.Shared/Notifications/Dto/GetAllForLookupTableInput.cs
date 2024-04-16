using Abp.Application.Services.Dto;

namespace Portal.Notifications.Dto
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}