using Abp.Application.Services.Dto;

namespace Portal.ProductNamespeace.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}