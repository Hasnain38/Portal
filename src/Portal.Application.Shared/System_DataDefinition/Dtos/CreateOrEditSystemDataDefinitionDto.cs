using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Portal.System_DataDefinition.Dtos
{
    public class CreateOrEditSystemDataDefinitionDto : EntityDto<int?>
    {

        public int DefTypeId { get; set; }

        public string DefValue { get; set; }

        public int DefParentId { get; set; }

        public int EntityId { get; set; }

    }
}