using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Portal.DataExporting.Excel.MiniExcel;
using Portal.OrderItemNamespeace.Dtos;
using Portal.Dto;
using Portal.Storage;

namespace Portal.OrderItemNamespeace.Exporting
{
    public class OrderItemsExcelExporter : MiniExcelExcelExporterBase, IOrderItemsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OrderItemsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOrderItemForViewDto> orderItems)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var orderItem in orderItems)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        {L("OrderId"), orderItem.OrderItem.OrderId},
                        {L("ProductId"), orderItem.OrderItem.ProductId},
                        {L("Quantity"), orderItem.OrderItem.Quantity},
                        {L("UnitPriceInclTax"), orderItem.OrderItem.UnitPriceInclTax},
                        {L("UnitPriceExclTax"), orderItem.OrderItem.UnitPriceExclTax},
                        {L("ItemWeight"), orderItem.OrderItem.ItemWeight},
                        {L("CreatedOnUtc"), orderItem.OrderItem.CreatedOnUtc},
                        {L("UpdatedOnUtc"), orderItem.OrderItem.UpdatedOnUtc},

                    });
            }

            return CreateExcelPackage("OrderItemsList.xlsx", items);

        }
    }
}