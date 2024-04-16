using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Portal
{
    public class PortalClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PortalClientModule).GetAssembly());
        }
    }
}
