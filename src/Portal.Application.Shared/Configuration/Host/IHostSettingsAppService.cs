﻿using System.Threading.Tasks;
using Abp.Application.Services;
using Portal.Configuration.Host.Dto;

namespace Portal.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
