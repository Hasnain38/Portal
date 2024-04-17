using System.Collections.Generic;
using Portal.ProductNamespeace.Dtos;
using Portal.Dto;

namespace Portal.ProductNamespeace.Exporting
{
    public interface IProductsExcelExporter
    {
        FileDto ExportToFile(List<GetProductForViewDto> products);
    }
}