using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Portal.Authorization.Users.Dto;

namespace Portal.Authorization.Users
{
    public interface IUserLoginAppService : IApplicationService
    {
        Task<PagedResultDto<UserLoginAttemptDto>> GetUserLoginAttempts(GetLoginAttemptsInput input);
    }
}
