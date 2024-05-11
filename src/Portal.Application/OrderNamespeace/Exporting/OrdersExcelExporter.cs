using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Portal.DataExporting.Excel.MiniExcel;
using Portal.OrderNamespeace.Dtos;
using Portal.Dto;
using Portal.Storage;

namespace Portal.OrderNamespeace.Exporting
{
    public class OrdersExcelExporter : MiniExcelExcelExporterBase, IOrdersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OrdersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOrderForViewDto> orders)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var order in orders)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        {L("OrderGuid"), order.Order.OrderGuid},
                        {L("StoreId"), order.Order.StoreId},
                        {L("CustomerId"), order.Order.CustomerId},
                        {L("BillingAddressId"), order.Order.BillingAddressId},
                        {L("ShippingAddressId"), order.Order.ShippingAddressId},
                        {L("PickupAddressId"), order.Order.PickupAddressId},
                        {L("PickupInStore"), order.Order.PickupInStore},
                        {L("OrderStatusId"), order.Order.OrderStatusId},
                        {L("ShippingStatusId"), order.Order.ShippingStatusId},
                        {L("PaymentStatusId"), order.Order.PaymentStatusId},
                        {L("OrderTax"), order.Order.OrderTax},
                        {L("OrderDiscount"), order.Order.OrderDiscount},
                        {L("OrderTotal"), order.Order.OrderTotal},
                        {L("CreatedOnUtc"), order.Order.CreatedOnUtc},
                        {L("UpdatedOnUtc"), order.Order.UpdatedOnUtc},
                        {L("CardTypeId"), order.Order.CardTypeId},
                        {L("CardName"), order.Order.CardName},
                        {L("CardNumber"), order.Order.CardNumber},
                        {L("Deleted"), order.Order.Deleted},

                    });
            }

            return CreateExcelPackage("OrdersList.xlsx", items);

        }
    }
}