using System.Threading.Tasks;
using Portal.Sessions.Dto;

namespace Portal.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
