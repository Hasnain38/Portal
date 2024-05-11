using System;
using Abp.Application.Services.Dto;

namespace Portal.SystemDataDefinitionType.Dtos
{
    public class System_DataDefinitionTypeDto : EntityDto
    {
        public string DefTypeValue { get; set; }

        public string DefTypeCode { get; set; }

        public int DefTypeParentId { get; set; }

    }
}