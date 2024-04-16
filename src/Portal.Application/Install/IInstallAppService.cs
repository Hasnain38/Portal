using System.Threading.Tasks;
using Abp.Application.Services;
using Portal.Install.Dto;

namespace Portal.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}