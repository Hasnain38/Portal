using System.Collections.Generic;
using Portal.TestEntityNamespeace.Dtos;
using Portal.Dto;

namespace Portal.TestEntityNamespeace.Exporting
{
    public interface ITestEntitiesExcelExporter
    {
        FileDto ExportToFile(List<GetTestEntityForViewDto> testEntities);
    }
}