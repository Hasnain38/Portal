using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Portal.SystemDataDefinitionType.Exporting;
using Portal.SystemDataDefinitionType.Dtos;
using Portal.Dto;
using Abp.Application.Services.Dto;
using Portal.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using Portal.Storage;

namespace Portal.SystemDataDefinitionType
{
    [AbpAuthorize(AppPermissions.Pages_System_DataDefinitionTypes)]
    public class System_DataDefinitionTypesAppService : PortalAppServiceBase, ISystem_DataDefinitionTypesAppService
    {
        private readonly IRepository<System_DataDefinitionType> _system_DataDefinitionTypeRepository;
        private readonly ISystem_DataDefinitionTypesExcelExporter _system_DataDefinitionTypesExcelExporter;

        public System_DataDefinitionTypesAppService(IRepository<System_DataDefinitionType> system_DataDefinitionTypeRepository, ISystem_DataDefinitionTypesExcelExporter system_DataDefinitionTypesExcelExporter)
        {
            _system_DataDefinitionTypeRepository = system_DataDefinitionTypeRepository;
            _system_DataDefinitionTypesExcelExporter = system_DataDefinitionTypesExcelExporter;

        }

        public virtual async Task<PagedResultDto<GetSystem_DataDefinitionTypeForViewDto>> GetAll(GetAllSystem_DataDefinitionTypesInput input)
        {

            var filteredSystem_DataDefinitionTypes = _system_DataDefinitionTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DefTypeValue.Contains(input.Filter) || e.DefTypeCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DefTypeValueFilter), e => e.DefTypeValue.Contains(input.DefTypeValueFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DefTypeCodeFilter), e => e.DefTypeCode.Contains(input.DefTypeCodeFilter))
                        .WhereIf(input.MinDefTypeParentIdFilter != null, e => e.DefTypeParentId >= input.MinDefTypeParentIdFilter)
                        .WhereIf(input.MaxDefTypeParentIdFilter != null, e => e.DefTypeParentId <= input.MaxDefTypeParentIdFilter);

            var pagedAndFilteredSystem_DataDefinitionTypes = filteredSystem_DataDefinitionTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var system_DataDefinitionTypes = from o in pagedAndFilteredSystem_DataDefinitionTypes
                                             select new
                                             {

                                                 o.DefTypeValue,
                                                 o.DefTypeCode,
                                                 o.DefTypeParentId,
                                                 Id = o.Id
                                             };

            var totalCount = await filteredSystem_DataDefinitionTypes.CountAsync();

            var dbList = await system_DataDefinitionTypes.ToListAsync();
            var results = new List<GetSystem_DataDefinitionTypeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetSystem_DataDefinitionTypeForViewDto()
                {
                    System_DataDefinitionType = new System_DataDefinitionTypeDto
                    {

                        DefTypeValue = o.DefTypeValue,
                        DefTypeCode = o.DefTypeCode,
                        DefTypeParentId = o.DefTypeParentId,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetSystem_DataDefinitionTypeForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetSystem_DataDefinitionTypeForViewDto> GetSystem_DataDefinitionTypeForView(int id)
        {
            var system_DataDefinitionType = await _system_DataDefinitionTypeRepository.GetAsync(id);

            var output = new GetSystem_DataDefinitionTypeForViewDto { System_DataDefinitionType = ObjectMapper.Map<System_DataDefinitionTypeDto>(system_DataDefinitionType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_System_DataDefinitionTypes_Edit)]
        public virtual async Task<GetSystem_DataDefinitionTypeForEditOutput> GetSystem_DataDefinitionTypeForEdit(EntityDto input)
        {
            var system_DataDefinitionType = await _system_DataDefinitionTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSystem_DataDefinitionTypeForEditOutput { System_DataDefinitionType = ObjectMapper.Map<CreateOrEditSystem_DataDefinitionTypeDto>(system_DataDefinitionType) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditSystem_DataDefinitionTypeDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_System_DataDefinitionTypes_Create)]
        protected virtual async Task Create(CreateOrEditSystem_DataDefinitionTypeDto input)
        {
            var system_DataDefinitionType = ObjectMapper.Map<System_DataDefinitionType>(input);

            await _system_DataDefinitionTypeRepository.InsertAsync(system_DataDefinitionType);

        }

        [AbpAuthorize(AppPermissions.Pages_System_DataDefinitionTypes_Edit)]
        protected virtual async Task Update(CreateOrEditSystem_DataDefinitionTypeDto input)
        {
            var system_DataDefinitionType = await _system_DataDefinitionTypeRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, system_DataDefinitionType);

        }

        [AbpAuthorize(AppPermissions.Pages_System_DataDefinitionTypes_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _system_DataDefinitionTypeRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetSystem_DataDefinitionTypesToExcel(GetAllSystem_DataDefinitionTypesForExcelInput input)
        {

            var filteredSystem_DataDefinitionTypes = _system_DataDefinitionTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DefTypeValue.Contains(input.Filter) || e.DefTypeCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DefTypeValueFilter), e => e.DefTypeValue.Contains(input.DefTypeValueFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DefTypeCodeFilter), e => e.DefTypeCode.Contains(input.DefTypeCodeFilter))
                        .WhereIf(input.MinDefTypeParentIdFilter != null, e => e.DefTypeParentId >= input.MinDefTypeParentIdFilter)
                        .WhereIf(input.MaxDefTypeParentIdFilter != null, e => e.DefTypeParentId <= input.MaxDefTypeParentIdFilter);

            var query = (from o in filteredSystem_DataDefinitionTypes
                         select new GetSystem_DataDefinitionTypeForViewDto()
                         {
                             System_DataDefinitionType = new System_DataDefinitionTypeDto
                             {
                                 DefTypeValue = o.DefTypeValue,
                                 DefTypeCode = o.DefTypeCode,
                                 DefTypeParentId = o.DefTypeParentId,
                                 Id = o.Id
                             }
                         });

            var system_DataDefinitionTypeListDtos = await query.ToListAsync();

            return _system_DataDefinitionTypesExcelExporter.ExportToFile(system_DataDefinitionTypeListDtos);
        }

    }
}