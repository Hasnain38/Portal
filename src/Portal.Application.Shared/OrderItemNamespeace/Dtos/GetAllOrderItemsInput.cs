using Abp.Application.Services.Dto;
using System;

namespace Portal.OrderItemNamespeace.Dtos
{
    public class GetAllOrderItemsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public long? MaxOrderIdFilter { get; set; }
        public long? MinOrderIdFilter { get; set; }

        public long? MaxProductIdFilter { get; set; }
        public long? MinProductIdFilter { get; set; }

        public int? MaxQuantityFilter { get; set; }
        public int? MinQuantityFilter { get; set; }

        public decimal? MaxUnitPriceInclTaxFilter { get; set; }
        public decimal? MinUnitPriceInclTaxFilter { get; set; }

        public decimal? MaxUnitPriceExclTaxFilter { get; set; }
        public decimal? MinUnitPriceExclTaxFilter { get; set; }

        public decimal? MaxItemWeightFilter { get; set; }
        public decimal? MinItemWeightFilter { get; set; }

        public DateTime? MaxCreatedOnUtcFilter { get; set; }
        public DateTime? MinCreatedOnUtcFilter { get; set; }

        public DateTime? MaxUpdatedOnUtcFilter { get; set; }
        public DateTime? MinUpdatedOnUtcFilter { get; set; }

    }
}