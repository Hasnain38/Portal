using System.Collections.Generic;
using Portal.System_DataDefinition.Dtos;
using Portal.Dto;

namespace Portal.System_DataDefinition.Exporting
{
    public interface ISystemDataDefinitionsExcelExporter
    {
        FileDto ExportToFile(List<GetSystemDataDefinitionForViewDto> systemDataDefinitions);
    }
}