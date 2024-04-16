using System.Threading.Tasks;
using Portal.Authorization.Users;

namespace Portal.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
