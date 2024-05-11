using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Portal.OrderItemNamespeace.Dtos
{
    public class GetOrderItemForEditOutput
    {
        public CreateOrEditOrderItemDto OrderItem { get; set; }

    }
}