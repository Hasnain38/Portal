using Portal.ProductNamespeace.Dtos;

using Abp.Extensions;

namespace Portal.Web.Areas.App.Models.Products
{
    public class CreateOrEditProductModalViewModel
    {
        public CreateOrEditProductDto Product { get; set; }

        public bool IsEditMode => Product.Id.HasValue;
    }
}