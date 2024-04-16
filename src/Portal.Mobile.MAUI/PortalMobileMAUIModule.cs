using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Portal.ApiClient;
using Portal.Mobile.MAUI.Core.ApiClient;

namespace Portal
{
    [DependsOn(typeof(PortalClientModule), typeof(AbpAutoMapperModule))]

    public class PortalMobileMAUIModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

            Configuration.ReplaceService<IApplicationContext, MAUIApplicationContext>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PortalMobileMAUIModule).GetAssembly());
        }
    }
}