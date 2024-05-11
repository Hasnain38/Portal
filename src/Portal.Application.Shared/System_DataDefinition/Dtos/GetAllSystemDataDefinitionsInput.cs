using Abp.Application.Services.Dto;
using System;

namespace Portal.System_DataDefinition.Dtos
{
    public class GetAllSystemDataDefinitionsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxDefTypeIdFilter { get; set; }
        public int? MinDefTypeIdFilter { get; set; }

        public string DefValueFilter { get; set; }

        public int? MaxDefParentIdFilter { get; set; }
        public int? MinDefParentIdFilter { get; set; }

        public int? MaxEntityIdFilter { get; set; }
        public int? MinEntityIdFilter { get; set; }

    }
}