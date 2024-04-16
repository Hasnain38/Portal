using Abp.AutoMapper;
using Portal.Authorization.Users;
using Portal.Authorization.Users.Dto;
using Portal.Web.Areas.App.Models.Common;

namespace Portal.Web.Areas.App.Models.Users
{
    [AutoMapFrom(typeof(GetUserPermissionsForEditOutput))]
    public class UserPermissionsEditViewModel : GetUserPermissionsForEditOutput, IPermissionsEditViewModel
    {
        public User User { get; set; }
    }
}