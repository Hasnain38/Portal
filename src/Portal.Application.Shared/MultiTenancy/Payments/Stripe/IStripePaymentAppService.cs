using System.Threading.Tasks;
using Abp.Application.Services;
using Portal.MultiTenancy.Payments.Dto;
using Portal.MultiTenancy.Payments.Stripe.Dto;

namespace Portal.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();
        
        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}