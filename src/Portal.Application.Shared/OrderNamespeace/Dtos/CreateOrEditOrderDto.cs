using Portal.GeneralEnums;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Portal.OrderNamespeace.Dtos
{
    public class CreateOrEditOrderDto : EntityDto<int?>
    {

        public Guid OrderGuid { get; set; }

        public long StoreId { get; set; }

        public long CustomerId { get; set; }

        public long BillingAddressId { get; set; }

        public long ShippingAddressId { get; set; }

        public long PickupAddressId { get; set; }

        public bool PickupInStore { get; set; }

        public OrderStatus OrderStatusId { get; set; }

        public ShippingStatus ShippingStatusId { get; set; }

        public PaymentStatus PaymentStatusId { get; set; }

        public decimal OrderTax { get; set; }

        public decimal OrderDiscount { get; set; }

        public decimal OrderTotal { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime UpdatedOnUtc { get; set; }

        public CardType CardTypeId { get; set; }

        public string CardName { get; set; }

        public string CardNumber { get; set; }

        public bool Deleted { get; set; }

    }
}