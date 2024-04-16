using Portal.Dto;

namespace Portal.WebHooks.Dto
{
    public class GetAllSendAttemptsInput : PagedInputDto
    {
        public string SubscriptionId { get; set; }
    }
}
