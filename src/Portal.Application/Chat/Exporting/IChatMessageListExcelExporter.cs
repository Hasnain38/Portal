using System.Collections.Generic;
using Abp;
using Portal.Chat.Dto;
using Portal.Dto;

namespace Portal.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
