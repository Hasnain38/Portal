using Abp.AutoMapper;
using Portal.Authorization.Roles.Dto;
using Portal.Web.Areas.App.Models.Common;

namespace Portal.Web.Areas.App.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class CreateOrEditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool IsEditMode => Role.Id.HasValue;
    }
}