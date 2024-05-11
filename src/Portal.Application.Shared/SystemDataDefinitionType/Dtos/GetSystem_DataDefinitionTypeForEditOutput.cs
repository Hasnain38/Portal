using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Portal.SystemDataDefinitionType.Dtos
{
    public class GetSystem_DataDefinitionTypeForEditOutput
    {
        public CreateOrEditSystem_DataDefinitionTypeDto System_DataDefinitionType { get; set; }

    }
}