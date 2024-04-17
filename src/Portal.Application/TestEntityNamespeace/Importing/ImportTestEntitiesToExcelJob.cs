using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Uow;
using Abp.IdentityFramework;
using Abp.Extensions;
using Abp.ObjectMapping;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Portal.TestEntityNamespeace.Importing.Dto;
using Portal.TestEntityNamespeace.Dtos;
using Portal.DataImporting.Excel;
using Portal.Notifications;
using Portal.Storage;

namespace Portal.TestEntityNamespeace
{

    public class ImportTestEntitiesToExcelJob(
        IObjectMapper objectMapper,
        IUnitOfWorkManager unitOfWorkManager,
        TestEntityListExcelDataReader dataReader,
        InvalidTestEntityExporter invalidEntityExporter,
        IAppNotifier appNotifier,
        IRepository<TestEntity> repository,
        IBinaryObjectManager binaryObjectManager)
        : ImportToExcelJobBase<ImportTestEntityDto, TestEntityListExcelDataReader, InvalidTestEntityExporter>(appNotifier,
            binaryObjectManager, unitOfWorkManager, dataReader, invalidEntityExporter)
    {
        public override string ErrorMessageKey => "FileCantBeConvertedToTestEntityList";

    public override string SuccessMessageKey => "AllTestEntitiesSuccessfullyImportedFromExcel";

    protected override async Task CreateEntityAsync(ImportTestEntityDto entity)
    {
        var testEntity = objectMapper.Map<TestEntity>(entity);

        // Add your custom validation here.

        await repository.InsertAsync(testEntity);
    }

}
}