using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Uow;
using Abp.IdentityFramework;
using Abp.Extensions;
using Abp.ObjectMapping;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Portal.OrderItemNamespeace.Importing.Dto;
using Portal.OrderItemNamespeace.Dtos;
using Portal.DataImporting.Excel;
using Portal.Notifications;
using Portal.Storage;

namespace Portal.OrderItemNamespeace
{

    public class ImportOrderItemsToExcelJob(
        IObjectMapper objectMapper,
        IUnitOfWorkManager unitOfWorkManager,
        OrderItemListExcelDataReader dataReader,
        InvalidOrderItemExporter invalidEntityExporter,
        IAppNotifier appNotifier,
        IRepository<OrderItem> repository,
        IBinaryObjectManager binaryObjectManager)
        : ImportToExcelJobBase<ImportOrderItemDto, OrderItemListExcelDataReader, InvalidOrderItemExporter>(appNotifier,
            binaryObjectManager, unitOfWorkManager, dataReader, invalidEntityExporter)
    {
        public override string ErrorMessageKey => "FileCantBeConvertedToOrderItemList";

    public override string SuccessMessageKey => "AllOrderItemsSuccessfullyImportedFromExcel";

    protected override async Task CreateEntityAsync(ImportOrderItemDto entity)
    {
        var orderItem = objectMapper.Map<OrderItem>(entity);

        // Add your custom validation here.

        await repository.InsertAsync(orderItem);
    }

}
}