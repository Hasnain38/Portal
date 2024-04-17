using Portal.TestEntityNamespeace.Dtos;

using Abp.Extensions;

namespace Portal.Web.Areas.App.Models.TestEntities
{
    public class CreateOrEditTestEntityModalViewModel
    {
        public CreateOrEditTestEntityDto TestEntity { get; set; }

        public bool IsEditMode => TestEntity.Id.HasValue;
    }
}