﻿using Abp.Application.Services.Dto;

namespace Portal.OrderNamespeace.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}