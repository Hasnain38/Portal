using Abp.Application.Services;
using Portal.Dto;
using Portal.Logging.Dto;

namespace Portal.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
