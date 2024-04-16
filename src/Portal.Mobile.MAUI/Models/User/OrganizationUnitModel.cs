using Abp.AutoMapper;
using Portal.Organizations.Dto;

namespace Portal.Mobile.MAUI.Models.User
{
    [AutoMapFrom(typeof(OrganizationUnitDto))]
    public class OrganizationUnitModel : OrganizationUnitDto
    {
        public bool IsAssigned { get; set; }
    }
}
