using Abp.Application.Services.Dto;
using System;

namespace Portal.TestEntityNamespeace.Dtos
{
    public class GetAllTestEntitiesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string TestNameFilter { get; set; }

    }
}