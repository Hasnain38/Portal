using System.Collections.Generic;
using Portal.SystemDataDefinitionType.Dtos;
using Portal.Dto;

namespace Portal.SystemDataDefinitionType.Exporting
{
    public interface ISystem_DataDefinitionTypesExcelExporter
    {
        FileDto ExportToFile(List<GetSystem_DataDefinitionTypeForViewDto> system_DataDefinitionTypes);
    }
}