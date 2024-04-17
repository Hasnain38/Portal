using System.Collections.Generic;
using Abp.Collections.Extensions;
using Portal.ProductNamespeace.Importing.Dto;
using Portal.DataExporting.Excel.MiniExcel;
using Portal.DataImporting.Excel;
using Portal.Dto;
using Portal.Storage;

namespace Portal.ProductNamespeace
{
    public class InvalidProductExporter(ITempFileCacheManager tempFileCacheManager)
        : MiniExcelExcelExporterBase(tempFileCacheManager), IExcelInvalidEntityExporter<ImportProductDto>
    {
        public FileDto ExportToFile(List<ImportProductDto> productList)
    {
        var items = new List<Dictionary<string, object>>();

        foreach (var product in productList)
        {
            items.Add(new Dictionary<string, object>()
                {
                    {L("Refuse Reason"), product.Exception},
                    {L("ProductName"), product.ProductName},
                    {L("ShortDescription"), product.ShortDescription},
                    {L("FullDescription"), product.FullDescription},
                    {L("ProductTemplateId"), product.ProductTemplateId},
                    {L("VendorId"), product.VendorId},
                    {L("ShowOnHomepage"), product.ShowOnHomepage},
                    {L("MetaKeywords"), product.MetaKeywords},
                    {L("MetaDescription"), product.MetaDescription},
                    {L("MetaTitle"), product.MetaTitle},
                    {L("AllowCustomerReviews"), product.AllowCustomerReviews},
                    {L("Sku"), product.Sku},
                    {L("IsGiftCard"), product.IsGiftCard},
                    {L("GiftCardTypeId"), product.GiftCardTypeId},
                    {L("WarehouseId"), product.WarehouseId},
                    {L("CreatedOnUtc"), product.CreatedOnUtc},
                    {L("UpdatedOnUtc"), product.UpdatedOnUtc},
                    {L("Deleted"), product.Deleted}
                });
        }

        return CreateExcelPackage("InvalidProductImportList.xlsx", items);
    }
}
}