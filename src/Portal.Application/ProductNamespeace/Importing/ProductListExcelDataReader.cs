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
using Portal.ProductNamespeace.Importing.Dto;

namespace Portal.ProductNamespeace
{
    public class ProductListExcelDataReader(ILocalizationManager localizationManager)
        : MiniExcelExcelImporterBase<ImportProductDto>, IExcelDataReader<ImportProductDto>
    {
        private readonly ILocalizationSource _localizationSource = localizationManager.GetSource(PortalConsts.LocalizationSourceName);

    public List<ImportProductDto> GetEntitiesFromExcel(byte[] fileBytes)
    {
        return ProcessExcelFile(fileBytes, ProcessExcelRow);
    }

    private ImportProductDto ProcessExcelRow(dynamic row)
    {

        var exceptionMessage = new StringBuilder();
        var product = new ImportProductDto();

        try
        {
            product.ProductName = GetRequiredValueFromRowOrNull(row, nameof(product.ProductName), exceptionMessage);
            product.ShortDescription = GetRequiredValueFromRowOrNull(row, nameof(product.ShortDescription), exceptionMessage);
            product.FullDescription = GetRequiredValueFromRowOrNull(row, nameof(product.FullDescription), exceptionMessage);
            product.ProductTemplateId = Convert.ToInt64(GetRequiredValueFromRowOrNull(row, nameof(product.ProductTemplateId), exceptionMessage));
            product.VendorId = Convert.ToInt64(GetRequiredValueFromRowOrNull(row, nameof(product.VendorId), exceptionMessage));
            product.ShowOnHomepage = Convert.ToBoolean(GetRequiredValueFromRowOrNull(row, nameof(product.ShowOnHomepage), exceptionMessage));
            product.MetaKeywords = GetRequiredValueFromRowOrNull(row, nameof(product.MetaKeywords), exceptionMessage);
            product.MetaDescription = GetRequiredValueFromRowOrNull(row, nameof(product.MetaDescription), exceptionMessage);
            product.MetaTitle = GetRequiredValueFromRowOrNull(row, nameof(product.MetaTitle), exceptionMessage);
            product.AllowCustomerReviews = Convert.ToBoolean(GetRequiredValueFromRowOrNull(row, nameof(product.AllowCustomerReviews), exceptionMessage));
            product.IsGiftCard = Convert.ToBoolean(GetRequiredValueFromRowOrNull(row, nameof(product.IsGiftCard), exceptionMessage));
            product.GiftCardTypeId = GetRequiredValueFromRowOrNull(row, nameof(product.GiftCardTypeId), exceptionMessage);
            product.WarehouseId = Convert.ToInt64(GetRequiredValueFromRowOrNull(row, nameof(product.WarehouseId), exceptionMessage));
            product.CreatedOnUtc = Convert.ToDateTime(GetRequiredValueFromRowOrNull(row, nameof(product.CreatedOnUtc), exceptionMessage));
            product.UpdatedOnUtc = Convert.ToDateTime(GetRequiredValueFromRowOrNull(row, nameof(product.UpdatedOnUtc), exceptionMessage));
            product.Deleted = Convert.ToBoolean(GetRequiredValueFromRowOrNull(row, nameof(product.Deleted), exceptionMessage));

        }
        catch (Exception exception)
        {
            product.Exception = exception.Message;
        }

        return product;
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