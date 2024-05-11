using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Portal.System_DataDefinition.Dtos
{
    public class GetSystemDataDefinitionForEditOutput
    {
        public CreateOrEditSystemDataDefinitionDto SystemDataDefinition { get; set; }

    }
}