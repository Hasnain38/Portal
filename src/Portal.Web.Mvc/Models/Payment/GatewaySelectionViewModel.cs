using System.Collections.Generic;
using System.Linq;
using Portal.MultiTenancy.Payments;
using Portal.MultiTenancy.Payments.Dto;

namespace Portal.Web.Models.Payment
{
    public class GatewaySelectionViewModel
    {
        public SubscriptionPaymentDto Payment { get; set; }
        
        public List<PaymentGatewayModel> PaymentGateways { get; set; }

        public bool AllowRecurringPaymentOption()
        {
            return Payment.AllowRecurringPayment() && PaymentGateways.Any(gateway => gateway.SupportsRecurringPayments);
        }
    }
}
