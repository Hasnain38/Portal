using Abp.AutoMapper;
using Portal.MultiTenancy;
using Portal.MultiTenancy.Dto;
using Portal.Web.Areas.App.Models.Common;

namespace Portal.Web.Areas.App.Models.Tenants
{
    [AutoMapFrom(typeof (GetTenantFeaturesEditOutput))]
    public class TenantFeaturesEditViewModel : GetTenantFeaturesEditOutput, IFeatureEditViewModel
    {
        public Tenant Tenant { get; set; }
    }
}