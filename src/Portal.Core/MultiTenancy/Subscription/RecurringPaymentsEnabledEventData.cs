using Abp.Events.Bus;

namespace Portal.MultiTenancy.Subscription
{
    public class RecurringPaymentsEnabledEventData : EventData
    {
        public int TenantId { get; set; }
    }
}