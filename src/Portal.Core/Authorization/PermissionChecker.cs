using Abp.Authorization;
using Portal.Authorization.Roles;
using Portal.Authorization.Users;

namespace Portal.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
