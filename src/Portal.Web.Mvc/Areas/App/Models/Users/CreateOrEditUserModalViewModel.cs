using System.Linq;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Portal.Authorization.Users.Dto;
using Portal.Security;
using Portal.Web.Areas.App.Models.Common;

namespace Portal.Web.Areas.App.Models.Users
{
    [AutoMapFrom(typeof(GetUserForEditOutput))]
    public class CreateOrEditUserModalViewModel : GetUserForEditOutput, IOrganizationUnitsEditViewModel
    {
        public bool CanChangeUserName => User.UserName != AbpUserBase.AdminUserName;

        public int AssignedRoleCount
        {
            get { return Roles.Count(r => r.IsAssigned); }
        }
        
        public int AssignedOrganizationUnitCount => MemberedOrganizationUnits.Count;

        public bool IsEditMode => User.Id.HasValue;

        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }
    }
}