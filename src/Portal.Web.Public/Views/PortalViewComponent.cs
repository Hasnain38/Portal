using Abp.AspNetCore.Mvc.ViewComponents;

namespace Portal.Web.Public.Views
{
    public abstract class PortalViewComponent : AbpViewComponent
    {
        protected PortalViewComponent()
        {
            LocalizationSourceName = PortalConsts.LocalizationSourceName;
        }
    }
}