using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Portal.TestEntityNamespeace.Exporting;
using Portal.TestEntityNamespeace.Dtos;
using Portal.Dto;
using Abp.Application.Services.Dto;
using Portal.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using Portal.Storage;

namespace Portal.TestEntityNamespeace
{
    [AbpAuthorize(AppPermissions.Pages_TestEntities)]
    public class TestEntitiesAppService : PortalAppServiceBase, ITestEntitiesAppService
    {
        private readonly IRepository<TestEntity> _testEntityRepository;
        private readonly ITestEntitiesExcelExporter _testEntitiesExcelExporter;

        public TestEntitiesAppService(IRepository<TestEntity> testEntityRepository, ITestEntitiesExcelExporter testEntitiesExcelExporter)
        {
            _testEntityRepository = testEntityRepository;
            _testEntitiesExcelExporter = testEntitiesExcelExporter;

        }

        public virtual async Task<PagedResultDto<GetTestEntityForViewDto>> GetAll(GetAllTestEntitiesInput input)
        {

            var filteredTestEntities = _testEntityRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TestName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TestNameFilter), e => e.TestName.Contains(input.TestNameFilter));

            var pagedAndFilteredTestEntities = filteredTestEntities
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var testEntities = from o in pagedAndFilteredTestEntities
                               select new
                               {

                                   o.TestName,
                                   Id = o.Id
                               };

            var totalCount = await filteredTestEntities.CountAsync();

            var dbList = await testEntities.ToListAsync();
            var results = new List<GetTestEntityForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTestEntityForViewDto()
                {
                    TestEntity = new TestEntityDto
                    {

                        TestName = o.TestName,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTestEntityForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetTestEntityForViewDto> GetTestEntityForView(int id)
        {
            var testEntity = await _testEntityRepository.GetAsync(id);

            var output = new GetTestEntityForViewDto { TestEntity = ObjectMapper.Map<TestEntityDto>(testEntity) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TestEntities_Edit)]
        public virtual async Task<GetTestEntityForEditOutput> GetTestEntityForEdit(EntityDto input)
        {
            var testEntity = await _testEntityRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTestEntityForEditOutput { TestEntity = ObjectMapper.Map<CreateOrEditTestEntityDto>(testEntity) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditTestEntityDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TestEntities_Create)]
        protected virtual async Task Create(CreateOrEditTestEntityDto input)
        {
            var testEntity = ObjectMapper.Map<TestEntity>(input);

            await _testEntityRepository.InsertAsync(testEntity);

        }

        [AbpAuthorize(AppPermissions.Pages_TestEntities_Edit)]
        protected virtual async Task Update(CreateOrEditTestEntityDto input)
        {
            var testEntity = await _testEntityRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, testEntity);

        }

        [AbpAuthorize(AppPermissions.Pages_TestEntities_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _testEntityRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetTestEntitiesToExcel(GetAllTestEntitiesForExcelInput input)
        {

            var filteredTestEntities = _testEntityRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TestName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TestNameFilter), e => e.TestName.Contains(input.TestNameFilter));

            var query = (from o in filteredTestEntities
                         select new GetTestEntityForViewDto()
                         {
                             TestEntity = new TestEntityDto
                             {
                                 TestName = o.TestName,
                                 Id = o.Id
                             }
                         });

            var testEntityListDtos = await query.ToListAsync();

            return _testEntitiesExcelExporter.ExportToFile(testEntityListDtos);
        }

    }
}