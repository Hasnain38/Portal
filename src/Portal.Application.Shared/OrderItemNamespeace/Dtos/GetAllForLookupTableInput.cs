using Abp.Application.Services.Dto;

namespace Portal.OrderItemNamespeace.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}