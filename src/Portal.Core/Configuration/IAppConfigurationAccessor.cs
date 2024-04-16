using Microsoft.Extensions.Configuration;

namespace Portal.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
