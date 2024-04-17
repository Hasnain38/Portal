using Abp.Application.Services.Dto;
using System;

namespace Portal.ProductNamespeace.Dtos
{
    public class GetAllProductsForExcelInput
    {
        public string Filter { get; set; }

        public string ProductNameFilter { get; set; }

        public string ShortDescriptionFilter { get; set; }

        public string FullDescriptionFilter { get; set; }

        public long? MaxProductTemplateIdFilter { get; set; }
        public long? MinProductTemplateIdFilter { get; set; }

        public long? MaxVendorIdFilter { get; set; }
        public long? MinVendorIdFilter { get; set; }

        public int? ShowOnHomepageFilter { get; set; }

        public string MetaKeywordsFilter { get; set; }

        public string MetaDescriptionFilter { get; set; }

        public string MetaTitleFilter { get; set; }

        public int? AllowCustomerReviewsFilter { get; set; }

        public Guid? SkuFilter { get; set; }

        public int? IsGiftCardFilter { get; set; }

        public string GiftCardTypeIdFilter { get; set; }

        public long? MaxWarehouseIdFilter { get; set; }
        public long? MinWarehouseIdFilter { get; set; }

        public DateTime? MaxCreatedOnUtcFilter { get; set; }
        public DateTime? MinCreatedOnUtcFilter { get; set; }

        public DateTime? MaxUpdatedOnUtcFilter { get; set; }
        public DateTime? MinUpdatedOnUtcFilter { get; set; }

        public int? DeletedFilter { get; set; }

    }
}