using System.Collections.Generic;
using Abp.Collections.Extensions;
using Portal.SystemDataDefinitionType.Importing.Dto;
using Portal.DataExporting.Excel.MiniExcel;
using Portal.DataImporting.Excel;
using Portal.Dto;
using Portal.Storage;

namespace Portal.SystemDataDefinitionType
{
    public class InvalidSystem_DataDefinitionTypeExporter(ITempFileCacheManager tempFileCacheManager)
        : MiniExcelExcelExporterBase(tempFileCacheManager), IExcelInvalidEntityExporter<ImportSystem_DataDefinitionTypeDto>
    {
        public FileDto ExportToFile(List<ImportSystem_DataDefinitionTypeDto> system_DataDefinitionTypeList)
    {
        var items = new List<Dictionary<string, object>>();

        foreach (var system_DataDefinitionType in system_DataDefinitionTypeList)
        {
            items.Add(new Dictionary<string, object>()
                {
                    {L("Refuse Reason"), system_DataDefinitionType.Exception},
                    {L("DefTypeValue"), system_DataDefinitionType.DefTypeValue},
                    {L("DefTypeCode"), system_DataDefinitionType.DefTypeCode},
                    {L("DefTypeParentId"), system_DataDefinitionType.DefTypeParentId}
                });
        }

        return CreateExcelPackage("InvalidSystem_DataDefinitionTypeImportList.xlsx", items);
    }
}
}