using Abp.Dependency;

namespace Portal.DashboardCustomization.Definitions.Cache
{
    public interface IDashboardDefinitionCacheManager : ITransientDependency
    {
        DashboardDefinition Get(string name);

        void Set(DashboardDefinition definition);
    }
}