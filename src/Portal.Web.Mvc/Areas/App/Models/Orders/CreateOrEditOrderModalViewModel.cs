using Portal.OrderNamespeace.Dtos;

using Abp.Extensions;

namespace Portal.Web.Areas.App.Models.Orders
{
    public class CreateOrEditOrderModalViewModel
    {
        public CreateOrEditOrderDto Order { get; set; }

        public bool IsEditMode => Order.Id.HasValue;
    }
}