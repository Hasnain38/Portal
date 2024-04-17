﻿using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Portal.ProductNamespeace.Dtos;
using Portal.Dto;

namespace Portal.ProductNamespeace
{
    public interface IProductsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductForViewDto>> GetAll(GetAllProductsInput input);

        Task<GetProductForViewDto> GetProductForView(int id);

        Task<GetProductForEditOutput> GetProductForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditProductDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetProductsToExcel(GetAllProductsForExcelInput input);

    }
}