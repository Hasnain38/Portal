using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Uow;
using Abp.IdentityFramework;
using Abp.Extensions;
using Abp.ObjectMapping;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Portal.ProductNamespeace.Importing.Dto;
using Portal.ProductNamespeace.Dtos;
using Portal.DataImporting.Excel;
using Portal.Notifications;
using Portal.Storage;

namespace Portal.ProductNamespeace
{

    public class ImportProductsToExcelJob(
        IObjectMapper objectMapper,
        IUnitOfWorkManager unitOfWorkManager,
        ProductListExcelDataReader dataReader,
        InvalidProductExporter invalidEntityExporter,
        IAppNotifier appNotifier,
        IRepository<Product> repository,
        IBinaryObjectManager binaryObjectManager)
        : ImportToExcelJobBase<ImportProductDto, ProductListExcelDataReader, InvalidProductExporter>(appNotifier,
            binaryObjectManager, unitOfWorkManager, dataReader, invalidEntityExporter)
    {
        public override string ErrorMessageKey => "FileCantBeConvertedToProductList";

    public override string SuccessMessageKey => "AllProductsSuccessfullyImportedFromExcel";

    protected override async Task CreateEntityAsync(ImportProductDto entity)
    {
        var product = objectMapper.Map<Product>(entity);

        // Add your custom validation here.

        await repository.InsertAsync(product);
    }

}
}