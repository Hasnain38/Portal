using Portal.System_DataDefinition.Dtos;

using Abp.Extensions;

namespace Portal.Web.Areas.App.Models.SystemDataDefinitions
{
    public class CreateOrEditSystemDataDefinitionModalViewModel
    {
        public CreateOrEditSystemDataDefinitionDto SystemDataDefinition { get; set; }

        public bool IsEditMode => SystemDataDefinition.Id.HasValue;
    }
}