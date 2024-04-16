using Abp.AutoMapper;
using Portal.Authorization.Users.Dto;

namespace Portal.Mobile.MAUI.Models.User
{
    [AutoMapFrom(typeof(CreateOrUpdateUserInput))]
    public class UserCreateOrUpdateModel : CreateOrUpdateUserInput
    {

    }
}
