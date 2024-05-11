using Abp.Application.Services.Dto;

namespace Portal.SystemDataDefinitionType.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}