using System;
using Abp.AutoMapper;
using Portal.DataImporting.Excel;
using Portal.ProductNamespeace.Dtos;

namespace Portal.ProductNamespeace.Importing.Dto
{
    [AutoMapTo(typeof(Product))]
    public class ImportProductDto : ImportFromExcelDto
    {
        public string ProductName { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public long ProductTemplateId { get; set; }
        public long VendorId { get; set; }
        public bool ShowOnHomepage { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public bool AllowCustomerReviews { get; set; }
        public Guid Sku { get; set; }
        public bool IsGiftCard { get; set; }
        public string GiftCardTypeId { get; set; }
        public long WarehouseId { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime UpdatedOnUtc { get; set; }
        public bool Deleted { get; set; }
        //TODO: Add navigation properties here
    }
}