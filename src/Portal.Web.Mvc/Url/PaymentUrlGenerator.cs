using Abp.Dependency;
using Abp.Extensions;
using Abp.Runtime.Session;
using Portal.Editions;
using Portal.ExtraProperties;
using Portal.MultiTenancy.Payments;
using Portal.Url;

namespace Portal.Web.Url
{
    public class PaymentUrlGenerator : IPaymentUrlGenerator, ITransientDependency
    {
        private readonly IWebUrlService _webUrlService;

        public PaymentUrlGenerator(
            IWebUrlService webUrlService)
        {
            _webUrlService = webUrlService;
        }

        public string CreatePaymentRequestUrl(SubscriptionPayment subscriptionPayment)
        {
            var webSiteRootAddress = _webUrlService.GetSiteRootAddress();

            var url = webSiteRootAddress.EnsureEndsWith('/') +
                      "Payment/GatewaySelection" +
                      "?paymentId=" + subscriptionPayment.Id;

            return url;
        }
    }
}