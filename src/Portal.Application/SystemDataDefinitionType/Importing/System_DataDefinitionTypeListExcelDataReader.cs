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
using Portal.SystemDataDefinitionType.Importing.Dto;

namespace Portal.SystemDataDefinitionType
{
    public class System_DataDefinitionTypeListExcelDataReader(ILocalizationManager localizationManager)
        : MiniExcelExcelImporterBase<ImportSystem_DataDefinitionTypeDto>, IExcelDataReader<ImportSystem_DataDefinitionTypeDto>
    {
        private readonly ILocalizationSource _localizationSource = localizationManager.GetSource(PortalConsts.LocalizationSourceName);

    public List<ImportSystem_DataDefinitionTypeDto> GetEntitiesFromExcel(byte[] fileBytes)
    {
        return ProcessExcelFile(fileBytes, ProcessExcelRow);
    }

    private ImportSystem_DataDefinitionTypeDto ProcessExcelRow(dynamic row)
    {

        var exceptionMessage = new StringBuilder();
        var system_DataDefinitionType = new ImportSystem_DataDefinitionTypeDto();

        try
        {
            system_DataDefinitionType.DefTypeValue = GetRequiredValueFromRowOrNull(row, nameof(system_DataDefinitionType.DefTypeValue), exceptionMessage);
            system_DataDefinitionType.DefTypeCode = GetRequiredValueFromRowOrNull(row, nameof(system_DataDefinitionType.DefTypeCode), exceptionMessage);
            system_DataDefinitionType.DefTypeParentId = Convert.ToInt32(GetRequiredValueFromRowOrNull(row, nameof(system_DataDefinitionType.DefTypeParentId), exceptionMessage));

        }
        catch (Exception exception)
        {
            system_DataDefinitionType.Exception = exception.Message;
        }

        return system_DataDefinitionType;
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