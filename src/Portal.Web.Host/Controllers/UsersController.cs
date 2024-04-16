using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.BackgroundJobs;
using Portal.Authorization;
using Portal.Authorization.Users.Importing;
using Portal.DataImporting.Excel;
using Portal.Storage;

namespace Portal.Web.Controllers;

[AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
public class UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
    : ExcelImportControllerBase(binaryObjectManager, backgroundJobManager)
{
    public override string ImportExcelPermission => AppPermissions.Pages_Administration_Users_Create;
    
    public override async Task EnqueueExcelImportJobAsync(ImportFromExcelJobArgs args)
    {
        await BackgroundJobManager.EnqueueAsync<ImportUsersToExcelJob, ImportFromExcelJobArgs>(args);
    }
}