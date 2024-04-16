using Abp.AutoMapper;
using Portal.MultiTenancy.Dto;

namespace Portal.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(RegisterTenantOutput))]
    public class TenantRegisterResultViewModel : RegisterTenantOutput
    {
        public string TenantLoginAddress { get; set; }
    }
}