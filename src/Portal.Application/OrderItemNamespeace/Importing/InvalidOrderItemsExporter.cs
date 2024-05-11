using System.Collections.Generic;
using Abp.Collections.Extensions;
using Portal.OrderItemNamespeace.Importing.Dto;
using Portal.DataExporting.Excel.MiniExcel;
using Portal.DataImporting.Excel;
using Portal.Dto;
using Portal.Storage;

namespace Portal.OrderItemNamespeace
{
    public class InvalidOrderItemExporter(ITempFileCacheManager tempFileCacheManager)
        : MiniExcelExcelExporterBase(tempFileCacheManager), IExcelInvalidEntityExporter<ImportOrderItemDto>
    {
        public FileDto ExportToFile(List<ImportOrderItemDto> orderItemList)
    {
        var items = new List<Dictionary<string, object>>();

        foreach (var orderItem in orderItemList)
        {
            items.Add(new Dictionary<string, object>()
                {
                    {L("Refuse Reason"), orderItem.Exception},
                    {L("OrderId"), orderItem.OrderId},
                    {L("ProductId"), orderItem.ProductId},
                    {L("Quantity"), orderItem.Quantity},
                    {L("UnitPriceInclTax"), orderItem.UnitPriceInclTax},
                    {L("UnitPriceExclTax"), orderItem.UnitPriceExclTax},
                    {L("ItemWeight"), orderItem.ItemWeight},
                    {L("CreatedOnUtc"), orderItem.CreatedOnUtc},
                    {L("UpdatedOnUtc"), orderItem.UpdatedOnUtc}
                });
        }

        return CreateExcelPackage("InvalidOrderItemImportList.xlsx", items);
    }
}
}