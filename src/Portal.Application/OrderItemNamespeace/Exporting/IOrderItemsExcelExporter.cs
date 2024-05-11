using System.Collections.Generic;
using Portal.OrderItemNamespeace.Dtos;
using Portal.Dto;

namespace Portal.OrderItemNamespeace.Exporting
{
    public interface IOrderItemsExcelExporter
    {
        FileDto ExportToFile(List<GetOrderItemForViewDto> orderItems);
    }
}