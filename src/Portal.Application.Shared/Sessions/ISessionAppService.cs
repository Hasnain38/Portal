using System.Threading.Tasks;
using Abp.Application.Services;
using Portal.Sessions.Dto;

namespace Portal.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
