using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Portal.ProductNamespeace
{
    [Table("Products")]
    public class Product : Entity
    {

        public virtual string ProductName { get; set; }

        public virtual string ShortDescription { get; set; }

        public virtual string FullDescription { get; set; }

        public virtual long ProductTemplateId { get; set; }

        public virtual long VendorId { get; set; }

        public virtual bool ShowOnHomepage { get; set; }

        public virtual string MetaKeywords { get; set; }

        public virtual string MetaDescription { get; set; }

        public virtual string MetaTitle { get; set; }

        public virtual bool AllowCustomerReviews { get; set; }

        public virtual Guid Sku { get; set; }

        public virtual bool IsGiftCard { get; set; }

        public virtual string GiftCardTypeId { get; set; }

        public virtual long WarehouseId { get; set; }

        public virtual DateTime CreatedOnUtc { get; set; }

        public virtual DateTime UpdatedOnUtc { get; set; }

        public virtual bool Deleted { get; set; }

    }
}