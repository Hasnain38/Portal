using System;
using Abp.Application.Services.Dto;

namespace Portal.OrderItemNamespeace.Dtos
{
    public class OrderItemDto : EntityDto
    {
        public long OrderId { get; set; }

        public long ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPriceInclTax { get; set; }

        public decimal UnitPriceExclTax { get; set; }

        public decimal ItemWeight { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime UpdatedOnUtc { get; set; }

    }
}