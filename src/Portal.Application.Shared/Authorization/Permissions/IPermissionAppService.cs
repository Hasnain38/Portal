using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Portal.Authorization.Permissions.Dto;

namespace Portal.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
