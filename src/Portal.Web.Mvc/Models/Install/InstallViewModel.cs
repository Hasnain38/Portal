using System.Collections.Generic;
using Abp.Localization;
using Portal.Install.Dto;

namespace Portal.Web.Models.Install
{
    public class InstallViewModel
    {
        public List<ApplicationLanguage> Languages { get; set; }

        public AppSettingsJsonDto AppSettingsJson { get; set; }
    }
}
