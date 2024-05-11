using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Uow;
using Abp.IdentityFramework;
using Abp.Extensions;
using Abp.ObjectMapping;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Portal.SystemDataDefinitionType.Importing.Dto;
using Portal.SystemDataDefinitionType.Dtos;
using Portal.DataImporting.Excel;
using Portal.Notifications;
using Portal.Storage;

namespace Portal.SystemDataDefinitionType
{

    public class ImportSystem_DataDefinitionTypesToExcelJob(
        IObjectMapper objectMapper,
        IUnitOfWorkManager unitOfWorkManager,
        System_DataDefinitionTypeListExcelDataReader dataReader,
        InvalidSystem_DataDefinitionTypeExporter invalidEntityExporter,
        IAppNotifier appNotifier,
        IRepository<System_DataDefinitionType> repository,
        IBinaryObjectManager binaryObjectManager)
        : ImportToExcelJobBase<ImportSystem_DataDefinitionTypeDto, System_DataDefinitionTypeListExcelDataReader, InvalidSystem_DataDefinitionTypeExporter>(appNotifier,
            binaryObjectManager, unitOfWorkManager, dataReader, invalidEntityExporter)
    {
        public override string ErrorMessageKey => "FileCantBeConvertedToSystem_DataDefinitionTypeList";

    public override string SuccessMessageKey => "AllSystem_DataDefinitionTypesSuccessfullyImportedFromExcel";

    protected override async Task CreateEntityAsync(ImportSystem_DataDefinitionTypeDto entity)
    {
        var system_DataDefinitionType = objectMapper.Map<System_DataDefinitionType>(entity);

        // Add your custom validation here.

        await repository.InsertAsync(system_DataDefinitionType);
    }

}
}