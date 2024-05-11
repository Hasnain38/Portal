using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Portal.System_DataDefinition.Dtos;
using Portal.Dto;

namespace Portal.System_DataDefinition
{
    public interface ISystemDataDefinitionsAppService : IApplicationService
    {
        Task<PagedResultDto<GetSystemDataDefinitionForViewDto>> GetAll(GetAllSystemDataDefinitionsInput input);

        Task<GetSystemDataDefinitionForViewDto> GetSystemDataDefinitionForView(int id);

        Task<GetSystemDataDefinitionForEditOutput> GetSystemDataDefinitionForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditSystemDataDefinitionDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetSystemDataDefinitionsToExcel(GetAllSystemDataDefinitionsForExcelInput input);

    }
}