using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Portal.ProductNamespeace.Exporting;
using Portal.ProductNamespeace.Dtos;
using Portal.Dto;
using Abp.Application.Services.Dto;
using Portal.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using Portal.Storage;

namespace Portal.ProductNamespeace
{
    [AbpAuthorize(AppPermissions.Pages_Products)]
    public class ProductsAppService : PortalAppServiceBase, IProductsAppService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IProductsExcelExporter _productsExcelExporter;

        public ProductsAppService(IRepository<Product> productRepository, IProductsExcelExporter productsExcelExporter)
        {
            _productRepository = productRepository;
            _productsExcelExporter = productsExcelExporter;

        }

        public virtual async Task<PagedResultDto<GetProductForViewDto>> GetAll(GetAllProductsInput input)
        {

            var filteredProducts = _productRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ProductName.Contains(input.Filter) || e.ShortDescription.Contains(input.Filter) || e.FullDescription.Contains(input.Filter) || e.MetaKeywords.Contains(input.Filter) || e.MetaDescription.Contains(input.Filter) || e.MetaTitle.Contains(input.Filter) || e.GiftCardTypeId.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductName.Contains(input.ProductNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ShortDescriptionFilter), e => e.ShortDescription.Contains(input.ShortDescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FullDescriptionFilter), e => e.FullDescription.Contains(input.FullDescriptionFilter))
                        .WhereIf(input.MinProductTemplateIdFilter != null, e => e.ProductTemplateId >= input.MinProductTemplateIdFilter)
                        .WhereIf(input.MaxProductTemplateIdFilter != null, e => e.ProductTemplateId <= input.MaxProductTemplateIdFilter)
                        .WhereIf(input.MinVendorIdFilter != null, e => e.VendorId >= input.MinVendorIdFilter)
                        .WhereIf(input.MaxVendorIdFilter != null, e => e.VendorId <= input.MaxVendorIdFilter)
                        .WhereIf(input.ShowOnHomepageFilter.HasValue && input.ShowOnHomepageFilter > -1, e => (input.ShowOnHomepageFilter == 1 && e.ShowOnHomepage) || (input.ShowOnHomepageFilter == 0 && !e.ShowOnHomepage))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaKeywordsFilter), e => e.MetaKeywords.Contains(input.MetaKeywordsFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaDescriptionFilter), e => e.MetaDescription.Contains(input.MetaDescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaTitleFilter), e => e.MetaTitle.Contains(input.MetaTitleFilter))
                        .WhereIf(input.AllowCustomerReviewsFilter.HasValue && input.AllowCustomerReviewsFilter > -1, e => (input.AllowCustomerReviewsFilter == 1 && e.AllowCustomerReviews) || (input.AllowCustomerReviewsFilter == 0 && !e.AllowCustomerReviews))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SkuFilter.ToString()), e => e.Sku.ToString() == input.SkuFilter.ToString())
                        .WhereIf(input.IsGiftCardFilter.HasValue && input.IsGiftCardFilter > -1, e => (input.IsGiftCardFilter == 1 && e.IsGiftCard) || (input.IsGiftCardFilter == 0 && !e.IsGiftCard))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GiftCardTypeIdFilter), e => e.GiftCardTypeId.Contains(input.GiftCardTypeIdFilter))
                        .WhereIf(input.MinWarehouseIdFilter != null, e => e.WarehouseId >= input.MinWarehouseIdFilter)
                        .WhereIf(input.MaxWarehouseIdFilter != null, e => e.WarehouseId <= input.MaxWarehouseIdFilter)
                        .WhereIf(input.MinCreatedOnUtcFilter != null, e => e.CreatedOnUtc >= input.MinCreatedOnUtcFilter)
                        .WhereIf(input.MaxCreatedOnUtcFilter != null, e => e.CreatedOnUtc <= input.MaxCreatedOnUtcFilter)
                        .WhereIf(input.MinUpdatedOnUtcFilter != null, e => e.UpdatedOnUtc >= input.MinUpdatedOnUtcFilter)
                        .WhereIf(input.MaxUpdatedOnUtcFilter != null, e => e.UpdatedOnUtc <= input.MaxUpdatedOnUtcFilter)
                        .WhereIf(input.DeletedFilter.HasValue && input.DeletedFilter > -1, e => (input.DeletedFilter == 1 && e.Deleted) || (input.DeletedFilter == 0 && !e.Deleted));

            var pagedAndFilteredProducts = filteredProducts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var products = from o in pagedAndFilteredProducts
                           select new
                           {

                               o.ProductName,
                               o.ShortDescription,
                               o.FullDescription,
                               o.ProductTemplateId,
                               o.VendorId,
                               o.ShowOnHomepage,
                               o.MetaKeywords,
                               o.MetaDescription,
                               o.MetaTitle,
                               o.AllowCustomerReviews,
                               o.Sku,
                               o.IsGiftCard,
                               o.GiftCardTypeId,
                               o.WarehouseId,
                               o.CreatedOnUtc,
                               o.UpdatedOnUtc,
                               o.Deleted,
                               Id = o.Id
                           };

            var totalCount = await filteredProducts.CountAsync();

            var dbList = await products.ToListAsync();
            var results = new List<GetProductForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductForViewDto()
                {
                    Product = new ProductDto
                    {

                        ProductName = o.ProductName,
                        ShortDescription = o.ShortDescription,
                        FullDescription = o.FullDescription,
                        ProductTemplateId = o.ProductTemplateId,
                        VendorId = o.VendorId,
                        ShowOnHomepage = o.ShowOnHomepage,
                        MetaKeywords = o.MetaKeywords,
                        MetaDescription = o.MetaDescription,
                        MetaTitle = o.MetaTitle,
                        AllowCustomerReviews = o.AllowCustomerReviews,
                        Sku = o.Sku,
                        IsGiftCard = o.IsGiftCard,
                        GiftCardTypeId = o.GiftCardTypeId,
                        WarehouseId = o.WarehouseId,
                        CreatedOnUtc = o.CreatedOnUtc,
                        UpdatedOnUtc = o.UpdatedOnUtc,
                        Deleted = o.Deleted,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetProductForViewDto> GetProductForView(int id)
        {
            var product = await _productRepository.GetAsync(id);

            var output = new GetProductForViewDto { Product = ObjectMapper.Map<ProductDto>(product) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Products_Edit)]
        public virtual async Task<GetProductForEditOutput> GetProductForEdit(EntityDto input)
        {
            var product = await _productRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductForEditOutput { Product = ObjectMapper.Map<CreateOrEditProductDto>(product) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditProductDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Products_Create)]
        protected virtual async Task Create(CreateOrEditProductDto input)
        {
            var product = ObjectMapper.Map<Product>(input);

            await _productRepository.InsertAsync(product);

        }

        [AbpAuthorize(AppPermissions.Pages_Products_Edit)]
        protected virtual async Task Update(CreateOrEditProductDto input)
        {
            var product = await _productRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, product);

        }

        [AbpAuthorize(AppPermissions.Pages_Products_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _productRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetProductsToExcel(GetAllProductsForExcelInput input)
        {

            var filteredProducts = _productRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ProductName.Contains(input.Filter) || e.ShortDescription.Contains(input.Filter) || e.FullDescription.Contains(input.Filter) || e.MetaKeywords.Contains(input.Filter) || e.MetaDescription.Contains(input.Filter) || e.MetaTitle.Contains(input.Filter) || e.GiftCardTypeId.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductName.Contains(input.ProductNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ShortDescriptionFilter), e => e.ShortDescription.Contains(input.ShortDescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FullDescriptionFilter), e => e.FullDescription.Contains(input.FullDescriptionFilter))
                        .WhereIf(input.MinProductTemplateIdFilter != null, e => e.ProductTemplateId >= input.MinProductTemplateIdFilter)
                        .WhereIf(input.MaxProductTemplateIdFilter != null, e => e.ProductTemplateId <= input.MaxProductTemplateIdFilter)
                        .WhereIf(input.MinVendorIdFilter != null, e => e.VendorId >= input.MinVendorIdFilter)
                        .WhereIf(input.MaxVendorIdFilter != null, e => e.VendorId <= input.MaxVendorIdFilter)
                        .WhereIf(input.ShowOnHomepageFilter.HasValue && input.ShowOnHomepageFilter > -1, e => (input.ShowOnHomepageFilter == 1 && e.ShowOnHomepage) || (input.ShowOnHomepageFilter == 0 && !e.ShowOnHomepage))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaKeywordsFilter), e => e.MetaKeywords.Contains(input.MetaKeywordsFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaDescriptionFilter), e => e.MetaDescription.Contains(input.MetaDescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaTitleFilter), e => e.MetaTitle.Contains(input.MetaTitleFilter))
                        .WhereIf(input.AllowCustomerReviewsFilter.HasValue && input.AllowCustomerReviewsFilter > -1, e => (input.AllowCustomerReviewsFilter == 1 && e.AllowCustomerReviews) || (input.AllowCustomerReviewsFilter == 0 && !e.AllowCustomerReviews))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SkuFilter.ToString()), e => e.Sku.ToString() == input.SkuFilter.ToString())
                        .WhereIf(input.IsGiftCardFilter.HasValue && input.IsGiftCardFilter > -1, e => (input.IsGiftCardFilter == 1 && e.IsGiftCard) || (input.IsGiftCardFilter == 0 && !e.IsGiftCard))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GiftCardTypeIdFilter), e => e.GiftCardTypeId.Contains(input.GiftCardTypeIdFilter))
                        .WhereIf(input.MinWarehouseIdFilter != null, e => e.WarehouseId >= input.MinWarehouseIdFilter)
                        .WhereIf(input.MaxWarehouseIdFilter != null, e => e.WarehouseId <= input.MaxWarehouseIdFilter)
                        .WhereIf(input.MinCreatedOnUtcFilter != null, e => e.CreatedOnUtc >= input.MinCreatedOnUtcFilter)
                        .WhereIf(input.MaxCreatedOnUtcFilter != null, e => e.CreatedOnUtc <= input.MaxCreatedOnUtcFilter)
                        .WhereIf(input.MinUpdatedOnUtcFilter != null, e => e.UpdatedOnUtc >= input.MinUpdatedOnUtcFilter)
                        .WhereIf(input.MaxUpdatedOnUtcFilter != null, e => e.UpdatedOnUtc <= input.MaxUpdatedOnUtcFilter)
                        .WhereIf(input.DeletedFilter.HasValue && input.DeletedFilter > -1, e => (input.DeletedFilter == 1 && e.Deleted) || (input.DeletedFilter == 0 && !e.Deleted));

            var query = (from o in filteredProducts
                         select new GetProductForViewDto()
                         {
                             Product = new ProductDto
                             {
                                 ProductName = o.ProductName,
                                 ShortDescription = o.ShortDescription,
                                 FullDescription = o.FullDescription,
                                 ProductTemplateId = o.ProductTemplateId,
                                 VendorId = o.VendorId,
                                 ShowOnHomepage = o.ShowOnHomepage,
                                 MetaKeywords = o.MetaKeywords,
                                 MetaDescription = o.MetaDescription,
                                 MetaTitle = o.MetaTitle,
                                 AllowCustomerReviews = o.AllowCustomerReviews,
                                 Sku = o.Sku,
                                 IsGiftCard = o.IsGiftCard,
                                 GiftCardTypeId = o.GiftCardTypeId,
                                 WarehouseId = o.WarehouseId,
                                 CreatedOnUtc = o.CreatedOnUtc,
                                 UpdatedOnUtc = o.UpdatedOnUtc,
                                 Deleted = o.Deleted,
                                 Id = o.Id
                             }
                         });

            var productListDtos = await query.ToListAsync();

            return _productsExcelExporter.ExportToFile(productListDtos);
        }

    }
}