using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Portal.DataExporting.Excel.MiniExcel;
using Portal.TestEntityNamespeace.Dtos;
using Portal.Dto;
using Portal.Storage;

namespace Portal.TestEntityNamespeace.Exporting
{
    public class TestEntitiesExcelExporter : MiniExcelExcelExporterBase, ITestEntitiesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TestEntitiesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTestEntityForViewDto> testEntities)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var testEntity in testEntities)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        {L("TestName"), testEntity.TestEntity.TestName},

                    });
            }

            return CreateExcelPackage("TestEntitiesList.xlsx", items);

        }
    }
}