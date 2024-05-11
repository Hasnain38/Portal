using System;
using Abp.AutoMapper;
using Portal.DataImporting.Excel;
using Portal.SystemDataDefinitionType.Dtos;

namespace Portal.SystemDataDefinitionType.Importing.Dto
{
    [AutoMapTo(typeof(System_DataDefinitionType))]
    public class ImportSystem_DataDefinitionTypeDto : ImportFromExcelDto
    {
        public string DefTypeValue { get; set; }
        public string DefTypeCode { get; set; }
        public int DefTypeParentId { get; set; }
        //TODO: Add navigation properties here
    }
}