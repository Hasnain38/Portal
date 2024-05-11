using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Portal.SystemDataDefinitionType.Dtos;
using Portal.Dto;

namespace Portal.SystemDataDefinitionType
{
    public interface ISystem_DataDefinitionTypesAppService : IApplicationService
    {
        Task<PagedResultDto<GetSystem_DataDefinitionTypeForViewDto>> GetAll(GetAllSystem_DataDefinitionTypesInput input);

        Task<GetSystem_DataDefinitionTypeForViewDto> GetSystem_DataDefinitionTypeForView(int id);

        Task<GetSystem_DataDefinitionTypeForEditOutput> GetSystem_DataDefinitionTypeForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditSystem_DataDefinitionTypeDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetSystem_DataDefinitionTypesToExcel(GetAllSystem_DataDefinitionTypesForExcelInput input);

    }
}