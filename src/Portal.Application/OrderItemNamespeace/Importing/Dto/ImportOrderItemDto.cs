using System;
using Abp.AutoMapper;
using Portal.DataImporting.Excel;
using Portal.OrderItemNamespeace.Dtos;

namespace Portal.OrderItemNamespeace.Importing.Dto
{
    [AutoMapTo(typeof(OrderItem))]
    public class ImportOrderItemDto : ImportFromExcelDto
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPriceInclTax { get; set; }
        public decimal UnitPriceExclTax { get; set; }
        public decimal ItemWeight { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime UpdatedOnUtc { get; set; }
        //TODO: Add navigation properties here
    }
}