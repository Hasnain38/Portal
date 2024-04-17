using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Portal.DataExporting.Excel.MiniExcel;
using Portal.ProductNamespeace.Dtos;
using Portal.Dto;
using Portal.Storage;

namespace Portal.ProductNamespeace.Exporting
{
    public class ProductsExcelExporter : MiniExcelExcelExporterBase, IProductsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductForViewDto> products)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var product in products)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        {L("ProductName"), product.Product.ProductName},
                        {L("ShortDescription"), product.Product.ShortDescription},
                        {L("FullDescription"), product.Product.FullDescription},
                        {L("ProductTemplateId"), product.Product.ProductTemplateId},
                        {L("VendorId"), product.Product.VendorId},
                        {L("ShowOnHomepage"), product.Product.ShowOnHomepage},
                        {L("MetaKeywords"), product.Product.MetaKeywords},
                        {L("MetaDescription"), product.Product.MetaDescription},
                        {L("MetaTitle"), product.Product.MetaTitle},
                        {L("AllowCustomerReviews"), product.Product.AllowCustomerReviews},
                        {L("Sku"), product.Product.Sku},
                        {L("IsGiftCard"), product.Product.IsGiftCard},
                        {L("GiftCardTypeId"), product.Product.GiftCardTypeId},
                        {L("WarehouseId"), product.Product.WarehouseId},
                        {L("CreatedOnUtc"), product.Product.CreatedOnUtc},
                        {L("UpdatedOnUtc"), product.Product.UpdatedOnUtc},
                        {L("Deleted"), product.Product.Deleted},

                    });
            }

            return CreateExcelPackage("ProductsList.xlsx", items);

        }
    }
}