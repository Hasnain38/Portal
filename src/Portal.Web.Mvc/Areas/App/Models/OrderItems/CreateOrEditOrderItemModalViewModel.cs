using Portal.OrderItemNamespeace.Dtos;

using Abp.Extensions;

namespace Portal.Web.Areas.App.Models.OrderItems
{
    public class CreateOrEditOrderItemModalViewModel
    {
        public CreateOrEditOrderItemDto OrderItem { get; set; }

        public bool IsEditMode => OrderItem.Id.HasValue;
    }
}