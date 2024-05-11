using Portal.GeneralEnums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Portal.OrderNamespeace
{
    [Table("Orders")]
    public class Order : Entity
    {

        public virtual Guid OrderGuid { get; set; }

        public virtual long StoreId { get; set; }

        public virtual long CustomerId { get; set; }

        public virtual long BillingAddressId { get; set; }

        public virtual long ShippingAddressId { get; set; }

        public virtual long PickupAddressId { get; set; }

        public virtual bool PickupInStore { get; set; }

        public virtual OrderStatus OrderStatusId { get; set; }

        public virtual ShippingStatus ShippingStatusId { get; set; }

        public virtual PaymentStatus PaymentStatusId { get; set; }

        public virtual decimal OrderTax { get; set; }

        public virtual decimal OrderDiscount { get; set; }

        public virtual decimal OrderTotal { get; set; }

        public virtual DateTime CreatedOnUtc { get; set; }

        public virtual DateTime UpdatedOnUtc { get; set; }

        public virtual CardType CardTypeId { get; set; }

        public virtual string CardName { get; set; }

        public virtual string CardNumber { get; set; }

        public virtual bool Deleted { get; set; }

    }
}