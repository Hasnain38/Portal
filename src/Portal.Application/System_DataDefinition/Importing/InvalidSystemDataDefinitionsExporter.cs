using System.Collections.Generic;
using Abp.Collections.Extensions;
using Portal.System_DataDefinition.Importing.Dto;
using Portal.DataExporting.Excel.MiniExcel;
using Portal.DataImporting.Excel;
using Portal.Dto;
using Portal.Storage;

namespace Portal.System_DataDefinition
{
    public class InvalidSystemDataDefinitionExporter(ITempFileCacheManager tempFileCacheManager)
        : MiniExcelExcelExporterBase(tempFileCacheManager), IExcelInvalidEntityExporter<ImportSystemDataDefinitionDto>
    {
        public FileDto ExportToFile(List<ImportSystemDataDefinitionDto> systemDataDefinitionList)
    {
        var items = new List<Dictionary<string, object>>();

        foreach (var systemDataDefinition in systemDataDefinitionList)
        {
            items.Add(new Dictionary<string, object>()
                {
                    {L("Refuse Reason"), systemDataDefinition.Exception},
                    {L("DefTypeId"), systemDataDefinition.DefTypeId},
                    {L("DefValue"), systemDataDefinition.DefValue},
                    {L("DefParentId"), systemDataDefinition.DefParentId},
                    {L("EntityId"), systemDataDefinition.EntityId}
                });
        }

        return CreateExcelPackage("InvalidSystemDataDefinitionImportList.xlsx", items);
    }
}
}