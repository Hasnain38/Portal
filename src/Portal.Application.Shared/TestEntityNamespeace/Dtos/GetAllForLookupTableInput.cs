using Abp.Application.Services.Dto;

namespace Portal.TestEntityNamespeace.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}