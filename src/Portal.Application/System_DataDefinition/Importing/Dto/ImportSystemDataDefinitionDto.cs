using System;
using Abp.AutoMapper;
using Portal.DataImporting.Excel;
using Portal.System_DataDefinition.Dtos;

namespace Portal.System_DataDefinition.Importing.Dto
{
    [AutoMapTo(typeof(SystemDataDefinition))]
    public class ImportSystemDataDefinitionDto : ImportFromExcelDto
    {
        public int DefTypeId { get; set; }
        public string DefValue { get; set; }
        public int DefParentId { get; set; }
        public int EntityId { get; set; }
        //TODO: Add navigation properties here
    }
}