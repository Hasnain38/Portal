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
using Portal.OrderItemNamespeace.Importing.Dto;

namespace Portal.OrderItemNamespeace
{
    public class OrderItemListExcelDataReader(ILocalizationManager localizationManager)
        : MiniExcelExcelImporterBase<ImportOrderItemDto>, IExcelDataReader<ImportOrderItemDto>
    {
        private readonly ILocalizationSource _localizationSource = localizationManager.GetSource(PortalConsts.LocalizationSourceName);

    public List<ImportOrderItemDto> GetEntitiesFromExcel(byte[] fileBytes)
    {
        return ProcessExcelFile(fileBytes, ProcessExcelRow);
    }

    private ImportOrderItemDto ProcessExcelRow(dynamic row)
    {

        var exceptionMessage = new StringBuilder();
        var orderItem = new ImportOrderItemDto();

        try
        {
            orderItem.OrderId = Convert.ToInt64(GetRequiredValueFromRowOrNull(row, nameof(orderItem.OrderId), exceptionMessage));
            orderItem.ProductId = Convert.ToInt64(GetRequiredValueFromRowOrNull(row, nameof(orderItem.ProductId), exceptionMessage));
            orderItem.Quantity = Convert.ToInt32(GetRequiredValueFromRowOrNull(row, nameof(orderItem.Quantity), exceptionMessage));
            orderItem.UnitPriceInclTax = Convert.ToDecimal(GetRequiredValueFromRowOrNull(row, nameof(orderItem.UnitPriceInclTax), exceptionMessage));
            orderItem.UnitPriceExclTax = Convert.ToDecimal(GetRequiredValueFromRowOrNull(row, nameof(orderItem.UnitPriceExclTax), exceptionMessage));
            orderItem.ItemWeight = Convert.ToDecimal(GetRequiredValueFromRowOrNull(row, nameof(orderItem.ItemWeight), exceptionMessage));
            orderItem.CreatedOnUtc = Convert.ToDateTime(GetRequiredValueFromRowOrNull(row, nameof(orderItem.CreatedOnUtc), exceptionMessage));
            orderItem.UpdatedOnUtc = Convert.ToDateTime(GetRequiredValueFromRowOrNull(row, nameof(orderItem.UpdatedOnUtc), exceptionMessage));

        }
        catch (Exception exception)
        {
            orderItem.Exception = exception.Message;
        }

        return orderItem;
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