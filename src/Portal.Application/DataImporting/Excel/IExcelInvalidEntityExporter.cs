using System.Collections.Generic;
using Abp.Dependency;
using Portal.Dto;

namespace Portal.DataImporting.Excel;

public interface IExcelInvalidEntityExporter<TEntityDto> : ITransientDependency
{
    FileDto ExportToFile(List<TEntityDto> entities);
}