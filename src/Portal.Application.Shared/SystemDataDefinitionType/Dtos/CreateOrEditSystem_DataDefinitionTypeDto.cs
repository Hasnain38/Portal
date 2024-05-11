using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Portal.SystemDataDefinitionType.Dtos
{
    public class CreateOrEditSystem_DataDefinitionTypeDto : EntityDto<int?>
    {

        public string DefTypeValue { get; set; }

        public string DefTypeCode { get; set; }

        public int DefTypeParentId { get; set; }

    }
}