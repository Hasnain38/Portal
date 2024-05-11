using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Portal.System_DataDefinition.Exporting;
using Portal.System_DataDefinition.Dtos;
using Portal.Dto;
using Abp.Application.Services.Dto;
using Portal.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using Portal.Storage;

namespace Portal.System_DataDefinition
{
    [AbpAuthorize(AppPermissions.Pages_SystemDataDefinitions)]
    public class SystemDataDefinitionsAppService : PortalAppServiceBase, ISystemDataDefinitionsAppService
    {
        private readonly IRepository<SystemDataDefinition> _systemDataDefinitionRepository;
        private readonly ISystemDataDefinitionsExcelExporter _systemDataDefinitionsExcelExporter;

        public SystemDataDefinitionsAppService(IRepository<SystemDataDefinition> systemDataDefinitionRepository, ISystemDataDefinitionsExcelExporter systemDataDefinitionsExcelExporter)
        {
            _systemDataDefinitionRepository = systemDataDefinitionRepository;
            _systemDataDefinitionsExcelExporter = systemDataDefinitionsExcelExporter;

        }

        public virtual async Task<PagedResultDto<GetSystemDataDefinitionForViewDto>> GetAll(GetAllSystemDataDefinitionsInput input)
        {

            var filteredSystemDataDefinitions = _systemDataDefinitionRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DefValue.Contains(input.Filter))
                        .WhereIf(input.MinDefTypeIdFilter != null, e => e.DefTypeId >= input.MinDefTypeIdFilter)
                        .WhereIf(input.MaxDefTypeIdFilter != null, e => e.DefTypeId <= input.MaxDefTypeIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DefValueFilter), e => e.DefValue.Contains(input.DefValueFilter))
                        .WhereIf(input.MinDefParentIdFilter != null, e => e.DefParentId >= input.MinDefParentIdFilter)
                        .WhereIf(input.MaxDefParentIdFilter != null, e => e.DefParentId <= input.MaxDefParentIdFilter)
                        .WhereIf(input.MinEntityIdFilter != null, e => e.EntityId >= input.MinEntityIdFilter)
                        .WhereIf(input.MaxEntityIdFilter != null, e => e.EntityId <= input.MaxEntityIdFilter);

            var pagedAndFilteredSystemDataDefinitions = filteredSystemDataDefinitions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var systemDataDefinitions = from o in pagedAndFilteredSystemDataDefinitions
                                        select new
                                        {

                                            o.DefTypeId,
                                            o.DefValue,
                                            o.DefParentId,
                                            o.EntityId,
                                            Id = o.Id
                                        };

            var totalCount = await filteredSystemDataDefinitions.CountAsync();

            var dbList = await systemDataDefinitions.ToListAsync();
            var results = new List<GetSystemDataDefinitionForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetSystemDataDefinitionForViewDto()
                {
                    SystemDataDefinition = new SystemDataDefinitionDto
                    {

                        DefTypeId = o.DefTypeId,
                        DefValue = o.DefValue,
                        DefParentId = o.DefParentId,
                        EntityId = o.EntityId,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetSystemDataDefinitionForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetSystemDataDefinitionForViewDto> GetSystemDataDefinitionForView(int id)
        {
            var systemDataDefinition = await _systemDataDefinitionRepository.GetAsync(id);

            var output = new GetSystemDataDefinitionForViewDto { SystemDataDefinition = ObjectMapper.Map<SystemDataDefinitionDto>(systemDataDefinition) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SystemDataDefinitions_Edit)]
        public virtual async Task<GetSystemDataDefinitionForEditOutput> GetSystemDataDefinitionForEdit(EntityDto input)
        {
            var systemDataDefinition = await _systemDataDefinitionRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSystemDataDefinitionForEditOutput { SystemDataDefinition = ObjectMapper.Map<CreateOrEditSystemDataDefinitionDto>(systemDataDefinition) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditSystemDataDefinitionDto input)
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

        [AbpAuthorize(AppPermissions.Pages_SystemDataDefinitions_Create)]
        protected virtual async Task Create(CreateOrEditSystemDataDefinitionDto input)
        {
            var systemDataDefinition = ObjectMapper.Map<SystemDataDefinition>(input);

            await _systemDataDefinitionRepository.InsertAsync(systemDataDefinition);

        }

        [AbpAuthorize(AppPermissions.Pages_SystemDataDefinitions_Edit)]
        protected virtual async Task Update(CreateOrEditSystemDataDefinitionDto input)
        {
            var systemDataDefinition = await _systemDataDefinitionRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, systemDataDefinition);

        }

        [AbpAuthorize(AppPermissions.Pages_SystemDataDefinitions_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _systemDataDefinitionRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetSystemDataDefinitionsToExcel(GetAllSystemDataDefinitionsForExcelInput input)
        {

            var filteredSystemDataDefinitions = _systemDataDefinitionRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DefValue.Contains(input.Filter))
                        .WhereIf(input.MinDefTypeIdFilter != null, e => e.DefTypeId >= input.MinDefTypeIdFilter)
                        .WhereIf(input.MaxDefTypeIdFilter != null, e => e.DefTypeId <= input.MaxDefTypeIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DefValueFilter), e => e.DefValue.Contains(input.DefValueFilter))
                        .WhereIf(input.MinDefParentIdFilter != null, e => e.DefParentId >= input.MinDefParentIdFilter)
                        .WhereIf(input.MaxDefParentIdFilter != null, e => e.DefParentId <= input.MaxDefParentIdFilter)
                        .WhereIf(input.MinEntityIdFilter != null, e => e.EntityId >= input.MinEntityIdFilter)
                        .WhereIf(input.MaxEntityIdFilter != null, e => e.EntityId <= input.MaxEntityIdFilter);

            var query = (from o in filteredSystemDataDefinitions
                         select new GetSystemDataDefinitionForViewDto()
                         {
                             SystemDataDefinition = new SystemDataDefinitionDto
                             {
                                 DefTypeId = o.DefTypeId,
                                 DefValue = o.DefValue,
                                 DefParentId = o.DefParentId,
                                 EntityId = o.EntityId,
                                 Id = o.Id
                             }
                         });

            var systemDataDefinitionListDtos = await query.ToListAsync();

            return _systemDataDefinitionsExcelExporter.ExportToFile(systemDataDefinitionListDtos);
        }

    }
}