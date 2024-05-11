using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Uow;
using Abp.IdentityFramework;
using Abp.Extensions;
using Abp.ObjectMapping;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Portal.OrderNamespeace.Importing.Dto;
using Portal.OrderNamespeace.Dtos;
using Portal.DataImporting.Excel;
using Portal.Notifications;
using Portal.Storage;

namespace Portal.OrderNamespeace
{

    public class ImportOrdersToExcelJob(
        IObjectMapper objectMapper,
        IUnitOfWorkManager unitOfWorkManager,
        OrderListExcelDataReader dataReader,
        InvalidOrderExporter invalidEntityExporter,
        IAppNotifier appNotifier,
        IRepository<Order> repository,
        IBinaryObjectManager binaryObjectManager)
        : ImportToExcelJobBase<ImportOrderDto, OrderListExcelDataReader, InvalidOrderExporter>(appNotifier,
            binaryObjectManager, unitOfWorkManager, dataReader, invalidEntityExporter)
    {
        public override string ErrorMessageKey => "FileCantBeConvertedToOrderList";

    public override string SuccessMessageKey => "AllOrdersSuccessfullyImportedFromExcel";

    protected override async Task CreateEntityAsync(ImportOrderDto entity)
    {
        var order = objectMapper.Map<Order>(entity);

        // Add your custom validation here.

        await repository.InsertAsync(order);
    }

}
}