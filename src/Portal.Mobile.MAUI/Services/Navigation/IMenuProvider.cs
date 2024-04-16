using Portal.Models.NavigationMenu;

namespace Portal.Services.Navigation
{
    public interface IMenuProvider
    {
        List<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}