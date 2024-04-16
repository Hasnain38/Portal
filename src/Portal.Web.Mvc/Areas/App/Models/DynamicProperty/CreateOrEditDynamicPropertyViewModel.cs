using System.Collections.Generic;
using Portal.DynamicEntityProperties.Dto;

namespace Portal.Web.Areas.App.Models.DynamicProperty
{
    public class CreateOrEditDynamicPropertyViewModel
    {
        public DynamicPropertyDto DynamicPropertyDto { get; set; }

        public List<string> AllowedInputTypes { get; set; }
    }
}
