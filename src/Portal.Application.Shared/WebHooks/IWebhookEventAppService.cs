using System.Threading.Tasks;
using Abp.Webhooks;

namespace Portal.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
