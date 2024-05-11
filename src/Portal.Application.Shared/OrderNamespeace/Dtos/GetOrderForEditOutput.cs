using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Portal.OrderNamespeace.Dtos
{
    public class GetOrderForEditOutput
    {
        public CreateOrEditOrderDto Order { get; set; }

    }
}