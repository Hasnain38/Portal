using System.Collections.Generic;
using Abp.Collections.Extensions;
using Portal.TestEntityNamespeace.Importing.Dto;
using Portal.DataExporting.Excel.MiniExcel;
using Portal.DataImporting.Excel;
using Portal.Dto;
using Portal.Storage;

namespace Portal.TestEntityNamespeace
{
    public class InvalidTestEntityExporter(ITempFileCacheManager tempFileCacheManager)
        : MiniExcelExcelExporterBase(tempFileCacheManager), IExcelInvalidEntityExporter<ImportTestEntityDto>
    {
        public FileDto ExportToFile(List<ImportTestEntityDto> testEntityList)
    {
        var items = new List<Dictionary<string, object>>();

        foreach (var testEntity in testEntityList)
        {
            items.Add(new Dictionary<string, object>()
                {
                    {L("Refuse Reason"), testEntity.Exception},
                    {L("TestName"), testEntity.TestName}
                });
        }

        return CreateExcelPackage("InvalidTestEntityImportList.xlsx", items);
    }
}
}