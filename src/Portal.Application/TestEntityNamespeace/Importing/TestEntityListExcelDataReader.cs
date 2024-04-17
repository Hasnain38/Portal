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
using Portal.TestEntityNamespeace.Importing.Dto;

namespace Portal.TestEntityNamespeace
{
    public class TestEntityListExcelDataReader(ILocalizationManager localizationManager)
        : MiniExcelExcelImporterBase<ImportTestEntityDto>, IExcelDataReader<ImportTestEntityDto>
    {
        private readonly ILocalizationSource _localizationSource = localizationManager.GetSource(PortalConsts.LocalizationSourceName);

    public List<ImportTestEntityDto> GetEntitiesFromExcel(byte[] fileBytes)
    {
        return ProcessExcelFile(fileBytes, ProcessExcelRow);
    }

    private ImportTestEntityDto ProcessExcelRow(dynamic row)
    {

        var exceptionMessage = new StringBuilder();
        var testEntity = new ImportTestEntityDto();

        try
        {
            testEntity.TestName = GetRequiredValueFromRowOrNull(row, nameof(testEntity.TestName), exceptionMessage);

        }
        catch (Exception exception)
        {
            testEntity.Exception = exception.Message;
        }

        return testEntity;
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