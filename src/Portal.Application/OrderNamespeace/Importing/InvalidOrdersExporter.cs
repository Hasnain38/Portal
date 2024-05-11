using System.Collections.Generic;
using Abp.Collections.Extensions;
using Portal.OrderNamespeace.Importing.Dto;
using Portal.DataExporting.Excel.MiniExcel;
using Portal.DataImporting.Excel;
using Portal.Dto;
using Portal.Storage;

namespace Portal.OrderNamespeace
{
    public class InvalidOrderExporter(ITempFileCacheManager tempFileCacheManager)
        : MiniExcelExcelExporterBase(tempFileCacheManager), IExcelInvalidEntityExporter<ImportOrderDto>
    {
        public FileDto ExportToFile(List<ImportOrderDto> orderList)
    {
        var items = new List<Dictionary<string, object>>();

        foreach (var order in orderList)
        {
            items.Add(new Dictionary<string, object>()
                {
                    {L("Refuse Reason"), order.Exception},
                    {L("OrderGuid"), order.OrderGuid},
                    {L("StoreId"), order.StoreId},
                    {L("CustomerId"), order.CustomerId},
                    {L("BillingAddressId"), order.BillingAddressId},
                    {L("ShippingAddressId"), order.ShippingAddressId},
                    {L("PickupAddressId"), order.PickupAddressId},
                    {L("PickupInStore"), order.PickupInStore},
                    {L("OrderStatusId"), order.OrderStatusId},
                    {L("ShippingStatusId"), order.ShippingStatusId},
                    {L("PaymentStatusId"), order.PaymentStatusId},
                    {L("OrderTax"), order.OrderTax},
                    {L("OrderDiscount"), order.OrderDiscount},
                    {L("OrderTotal"), order.OrderTotal},
                    {L("CreatedOnUtc"), order.CreatedOnUtc},
                    {L("UpdatedOnUtc"), order.UpdatedOnUtc},
                    {L("CardTypeId"), order.CardTypeId},
                    {L("CardName"), order.CardName},
                    {L("CardNumber"), order.CardNumber},
                    {L("Deleted"), order.Deleted}
                });
        }

        return CreateExcelPackage("InvalidOrderImportList.xlsx", items);
    }
}
}