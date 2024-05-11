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
using Portal.System_DataDefinition.Importing.Dto;

namespace Portal.System_DataDefinition
{
    public class SystemDataDefinitionListExcelDataReader(ILocalizationManager localizationManager)
        : MiniExcelExcelImporterBase<ImportSystemDataDefinitionDto>, IExcelDataReader<ImportSystemDataDefinitionDto>
    {
        private readonly ILocalizationSource _localizationSource = localizationManager.GetSource(PortalConsts.LocalizationSourceName);

    public List<ImportSystemDataDefinitionDto> GetEntitiesFromExcel(byte[] fileBytes)
    {
        return ProcessExcelFile(fileBytes, ProcessExcelRow);
    }

    private ImportSystemDataDefinitionDto ProcessExcelRow(dynamic row)
    {

        var exceptionMessage = new StringBuilder();
        var systemDataDefinition = new ImportSystemDataDefinitionDto();

        try
        {
            systemDataDefinition.DefTypeId = Convert.ToInt32(GetRequiredValueFromRowOrNull(row, nameof(systemDataDefinition.DefTypeId), exceptionMessage));
            systemDataDefinition.DefValue = GetRequiredValueFromRowOrNull(row, nameof(systemDataDefinition.DefValue), exceptionMessage);
            systemDataDefinition.DefParentId = Convert.ToInt32(GetRequiredValueFromRowOrNull(row, nameof(systemDataDefinition.DefParentId), exceptionMessage));
            systemDataDefinition.EntityId = Convert.ToInt32(GetRequiredValueFromRowOrNull(row, nameof(systemDataDefinition.EntityId), exceptionMessage));

        }
        catch (Exception exception)
        {
            systemDataDefinition.Exception = exception.Message;
        }

        return systemDataDefinition;
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