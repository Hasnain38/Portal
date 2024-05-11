using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Portal.DataExporting.Excel.MiniExcel;
using Portal.System_DataDefinition.Dtos;
using Portal.Dto;
using Portal.Storage;

namespace Portal.System_DataDefinition.Exporting
{
    public class SystemDataDefinitionsExcelExporter : MiniExcelExcelExporterBase, ISystemDataDefinitionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SystemDataDefinitionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSystemDataDefinitionForViewDto> systemDataDefinitions)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var systemDataDefinition in systemDataDefinitions)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        {L("DefTypeId"), systemDataDefinition.SystemDataDefinition.DefTypeId},
                        {L("DefValue"), systemDataDefinition.SystemDataDefinition.DefValue},
                        {L("DefParentId"), systemDataDefinition.SystemDataDefinition.DefParentId},
                        {L("EntityId"), systemDataDefinition.SystemDataDefinition.EntityId},

                    });
            }

            return CreateExcelPackage("SystemDataDefinitionsList.xlsx", items);

        }
    }
}