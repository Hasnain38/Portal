using Portal.MultiTenancy.Dto;
using Portal.Sessions.Dto;

namespace Portal.Web.Areas.App.Models.Editions
{
    public class SubscriptionDashboardViewModel
    {
        public GetCurrentLoginInformationsOutput LoginInformations { get; set; }
        
        public EditionsSelectOutput Editions { get; set; }
    }
}
