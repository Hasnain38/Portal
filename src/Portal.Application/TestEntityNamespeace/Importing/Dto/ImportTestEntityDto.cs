using System;
using Abp.AutoMapper;
using Portal.DataImporting.Excel;
using Portal.TestEntityNamespeace.Dtos;

namespace Portal.TestEntityNamespeace.Importing.Dto
{
    [AutoMapTo(typeof(TestEntity))]
    public class ImportTestEntityDto : ImportFromExcelDto
    {
        public string TestName { get; set; }
        //TODO: Add navigation properties here
    }
}