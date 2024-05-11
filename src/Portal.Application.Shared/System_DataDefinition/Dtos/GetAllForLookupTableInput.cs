using Abp.Application.Services.Dto;

namespace Portal.System_DataDefinition.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}