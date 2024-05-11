using Portal.SystemDataDefinitionType.Dtos;

using Abp.Extensions;

namespace Portal.Web.Areas.App.Models.System_DataDefinitionTypes
{
    public class CreateOrEditSystem_DataDefinitionTypeModalViewModel
    {
        public CreateOrEditSystem_DataDefinitionTypeDto System_DataDefinitionType { get; set; }

        public bool IsEditMode => System_DataDefinitionType.Id.HasValue;
    }
}