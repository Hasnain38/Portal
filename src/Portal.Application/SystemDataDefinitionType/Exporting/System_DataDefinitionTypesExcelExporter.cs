using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Portal.DataExporting.Excel.MiniExcel;
using Portal.SystemDataDefinitionType.Dtos;
using Portal.Dto;
using Portal.Storage;

namespace Portal.SystemDataDefinitionType.Exporting
{
    public class System_DataDefinitionTypesExcelExporter : MiniExcelExcelExporterBase, ISystem_DataDefinitionTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public System_DataDefinitionTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSystem_DataDefinitionTypeForViewDto> system_DataDefinitionTypes)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var system_DataDefinitionType in system_DataDefinitionTypes)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        {L("DefTypeValue"), system_DataDefinitionType.System_DataDefinitionType.DefTypeValue},
                        {L("DefTypeCode"), system_DataDefinitionType.System_DataDefinitionType.DefTypeCode},
                        {L("DefTypeParentId"), system_DataDefinitionType.System_DataDefinitionType.DefTypeParentId},

                    });
            }

            return CreateExcelPackage("System_DataDefinitionTypesList.xlsx", items);

        }
    }
}