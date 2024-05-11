using System;
using Abp.Application.Services.Dto;

namespace Portal.System_DataDefinition.Dtos
{
    public class SystemDataDefinitionDto : EntityDto
    {
        public int DefTypeId { get; set; }

        public string DefValue { get; set; }

        public int DefParentId { get; set; }

        public int EntityId { get; set; }

    }
}