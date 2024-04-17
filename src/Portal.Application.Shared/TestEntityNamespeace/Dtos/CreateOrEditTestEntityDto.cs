using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Portal.TestEntityNamespeace.Dtos
{
    public class CreateOrEditTestEntityDto : EntityDto<int?>
    {

        public string TestName { get; set; }

    }
}