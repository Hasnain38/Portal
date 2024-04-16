using System.Collections.Generic;

namespace Portal.Authorization.Roles.Dto
{
    public class GetRolesInput
    {
        public List<string> Permissions { get; set; }
    }
}
