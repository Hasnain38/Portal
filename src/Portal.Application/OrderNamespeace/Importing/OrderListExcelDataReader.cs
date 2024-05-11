using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using System.Linq;
using Abp.Collections.Extensions;
using Portal.DataExporting.Excel.MiniExcel;
using Portal.DataImporting.Excel;
using Portal.OrderNamespeace.Importing.Dto;

namespace Portal.OrderNamespeace
{
    public class OrderListExcelDataReader(ILocalizationManager localizationManager)
        : MiniExcelExcelImporterBase<ImportOrderDto>, IExcelDataReader<ImportOrderDto>
    {
        private readonly ILocalizationSource _localizationSource = localizationManager.GetSource(PortalConsts.LocalizationSourceName);

    public List<ImportOrderDto> GetEntitiesFromExcel(byte[] fileBytes)
    {
        return ProcessExcelFile(fileBytes, ProcessExcelRow);
    }

    private ImportOrderDto ProcessExcelRow(dynamic row)
    {

        var exceptionMessage = new StringBuilder();
        var order = new ImportOrderDto();

        try
        {
            order.StoreId = Convert.ToInt64(GetRequiredValueFromRowOrNull(row, nameof(order.StoreId), exceptionMessage));
            order.CustomerId = Convert.ToInt64(GetRequiredValueFromRowOrNull(row, nameof(order.CustomerId), exceptionMessage));
            order.BillingAddressId = Convert.ToInt64(GetRequiredValueFromRowOrNull(row, nameof(order.BillingAddressId), exceptionMessage));
            order.ShippingAddressId = Convert.ToInt64(GetRequiredValueFromRowOrNull(row, nameof(order.ShippingAddressId), exceptionMessage));
            order.PickupAddressId = Convert.ToInt64(GetRequiredValueFromRowOrNull(row, nameof(order.PickupAddressId), exceptionMessage));
            order.PickupInStore = Convert.ToBoolean(GetRequiredValueFromRowOrNull(row, nameof(order.PickupInStore), exceptionMessage));
            order.OrderTax = Convert.ToDecimal(GetRequiredValueFromRowOrNull(row, nameof(order.OrderTax), exceptionMessage));
            order.OrderDiscount = Convert.ToDecimal(GetRequiredValueFromRowOrNull(row, nameof(order.OrderDiscount), exceptionMessage));
            order.OrderTotal = Convert.ToDecimal(GetRequiredValueFromRowOrNull(row, nameof(order.OrderTotal), exceptionMessage));
            order.CreatedOnUtc = Convert.ToDateTime(GetRequiredValueFromRowOrNull(row, nameof(order.CreatedOnUtc), exceptionMessage));
            order.UpdatedOnUtc = Convert.ToDateTime(GetRequiredValueFromRowOrNull(row, nameof(order.UpdatedOnUtc), exceptionMessage));
            order.CardName = GetRequiredValueFromRowOrNull(row, nameof(order.CardName), exceptionMessage);
            order.CardNumber = GetRequiredValueFromRowOrNull(row, nameof(order.CardNumber), exceptionMessage);
            order.Deleted = Convert.ToBoolean(GetRequiredValueFromRowOrNull(row, nameof(order.Deleted), exceptionMessage));

        }
        catch (Exception exception)
        {
            order.Exception = exception.Message;
        }

        return order;
    }

    private string GetRequiredValueFromRowOrNull(
        dynamic row,
        string columnName,
        StringBuilder exceptionMessage)
    {
        var cellValue = (row as ExpandoObject).GetOrDefault(columnName)?.ToString();
        if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue))
        {
            return cellValue;
        }

        exceptionMessage.Append(GetLocalizedExceptionMessagePart(columnName));
        return null;
    }

    private string GetOptionalValueFromRowOrNull(dynamic row, string columnName, StringBuilder exceptionMessage)
    {
        var cellValue = (row as ExpandoObject).GetOrDefault(columnName)?.ToString();
        if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue))
        {
            return cellValue;
        }

        exceptionMessage.Append(GetLocalizedExceptionMessagePart(columnName));
        return String.Empty;
    }

    private string GetLocalizedExceptionMessagePart(string parameter)
    {
        return _localizationSource.GetString("{0}IsInvalid", _localizationSource.GetString(parameter)) + "; ";
    }

}
}