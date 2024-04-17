using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Portal.TestEntityNamespeace.Dtos
{
    public class GetTestEntityForEditOutput
    {
        public CreateOrEditTestEntityDto TestEntity { get; set; }

    }
}