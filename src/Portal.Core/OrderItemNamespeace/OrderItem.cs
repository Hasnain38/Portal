using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Portal.OrderItemNamespeace
{
    [Table("OrderItems")]
    public class OrderItem : Entity
    {

        public virtual long OrderId { get; set; }

        public virtual long ProductId { get; set; }

        public virtual int Quantity { get; set; }

        public virtual decimal UnitPriceInclTax { get; set; }

        public virtual decimal UnitPriceExclTax { get; set; }

        public virtual decimal ItemWeight { get; set; }

        public virtual DateTime CreatedOnUtc { get; set; }

        public virtual DateTime UpdatedOnUtc { get; set; }

    }
}