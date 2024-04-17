using Abp.Application.Services.Dto;
using System;

namespace Portal.TestEntityNamespeace.Dtos
{
    public class GetAllTestEntitiesForExcelInput
    {
        public string Filter { get; set; }

        public string TestNameFilter { get; set; }

    }
}