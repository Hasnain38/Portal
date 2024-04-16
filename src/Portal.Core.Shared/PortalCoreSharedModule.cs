using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Portal
{
    public class PortalCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PortalCoreSharedModule).GetAssembly());
        }
    }
}