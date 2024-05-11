using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Uow;
using Abp.IdentityFramework;
using Abp.Extensions;
using Abp.ObjectMapping;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Portal.System_DataDefinition.Importing.Dto;
using Portal.System_DataDefinition.Dtos;
using Portal.DataImporting.Excel;
using Portal.Notifications;
using Portal.Storage;

namespace Portal.System_DataDefinition
{

    public class ImportSystemDataDefinitionsToExcelJob(
        IObjectMapper objectMapper,
        IUnitOfWorkManager unitOfWorkManager,
        SystemDataDefinitionListExcelDataReader dataReader,
        InvalidSystemDataDefinitionExporter invalidEntityExporter,
        IAppNotifier appNotifier,
        IRepository<SystemDataDefinition> repository,
        IBinaryObjectManager binaryObjectManager)
        : ImportToExcelJobBase<ImportSystemDataDefinitionDto, SystemDataDefinitionListExcelDataReader, InvalidSystemDataDefinitionExporter>(appNotifier,
            binaryObjectManager, unitOfWorkManager, dataReader, invalidEntityExporter)
    {
        public override string ErrorMessageKey => "FileCantBeConvertedToSystemDataDefinitionList";

    public override string SuccessMessageKey => "AllSystemDataDefinitionsSuccessfullyImportedFromExcel";

    protected override async Task CreateEntityAsync(ImportSystemDataDefinitionDto entity)
    {
        var systemDataDefinition = objectMapper.Map<SystemDataDefinition>(entity);

        // Add your custom validation here.

        await repository.InsertAsync(systemDataDefinition);
    }

}
}