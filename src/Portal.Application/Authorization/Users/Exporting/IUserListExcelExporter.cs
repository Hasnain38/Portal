using System.Collections.Generic;
using Portal.Authorization.Users.Dto;
using Portal.Dto;

namespace Portal.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos, List<string> selectedColumns);
    }
}