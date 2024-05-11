using Abp.Application.Services.Dto;
using System;

namespace Portal.SystemDataDefinitionType.Dtos
{
    public class GetAllSystem_DataDefinitionTypesForExcelInput
    {
        public string Filter { get; set; }

        public string DefTypeValueFilter { get; set; }

        public string DefTypeCodeFilter { get; set; }

        public int? MaxDefTypeParentIdFilter { get; set; }
        public int? MinDefTypeParentIdFilter { get; set; }

    }
}