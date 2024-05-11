using System.Collections.Generic;
using Portal.OrderNamespeace.Dtos;
using Portal.Dto;

namespace Portal.OrderNamespeace.Exporting
{
    public interface IOrdersExcelExporter
    {
        FileDto ExportToFile(List<GetOrderForViewDto> orders);
    }
}