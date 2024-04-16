using Abp.Events.Bus.Handlers;
using Portal.MultiTenancy.Subscription;

namespace Portal.MultiTenancy.Payments
{
    public interface ISupportsRecurringPayments : 
        IEventHandler<RecurringPaymentsDisabledEventData>, 
        IEventHandler<RecurringPaymentsEnabledEventData>,
        IEventHandler<SubscriptionUpdatedEventData>,
        IEventHandler<SubscriptionCancelledEventData>
    {

    }
}
