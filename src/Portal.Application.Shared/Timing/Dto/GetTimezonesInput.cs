using Abp.Configuration;

namespace Portal.Timing.Dto
{
    public class GetTimezonesInput
    {
        public SettingScopes DefaultTimezoneScope { get; set; }
    }
}
