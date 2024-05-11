using Abp.Application.Services.Dto;
using System;

namespace Portal.OrderNamespeace.Dtos
{
    public class GetAllOrdersInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public Guid? OrderGuidFilter { get; set; }

        public long? MaxStoreIdFilter { get; set; }
        public long? MinStoreIdFilter { get; set; }

        public long? MaxCustomerIdFilter { get; set; }
        public long? MinCustomerIdFilter { get; set; }

        public long? MaxBillingAddressIdFilter { get; set; }
        public long? MinBillingAddressIdFilter { get; set; }

        public long? MaxShippingAddressIdFilter { get; set; }
        public long? MinShippingAddressIdFilter { get; set; }

        public long? MaxPickupAddressIdFilter { get; set; }
        public long? MinPickupAddressIdFilter { get; set; }

        public int? PickupInStoreFilter { get; set; }

        public int? OrderStatusIdFilter { get; set; }

        public int? ShippingStatusIdFilter { get; set; }

        public int? PaymentStatusIdFilter { get; set; }

        public decimal? MaxOrderTaxFilter { get; set; }
        public decimal? MinOrderTaxFilter { get; set; }

        public decimal? MaxOrderDiscountFilter { get; set; }
        public decimal? MinOrderDiscountFilter { get; set; }

        public decimal? MaxOrderTotalFilter { get; set; }
        public decimal? MinOrderTotalFilter { get; set; }

        public DateTime? MaxCreatedOnUtcFilter { get; set; }
        public DateTime? MinCreatedOnUtcFilter { get; set; }

        public DateTime? MaxUpdatedOnUtcFilter { get; set; }
        public DateTime? MinUpdatedOnUtcFilter { get; set; }

        public int? CardTypeIdFilter { get; set; }

        public string CardNameFilter { get; set; }

        public string CardNumberFilter { get; set; }

        public int? DeletedFilter { get; set; }

    }
}