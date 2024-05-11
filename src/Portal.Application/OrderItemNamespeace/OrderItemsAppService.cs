using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Portal.OrderItemNamespeace.Exporting;
using Portal.OrderItemNamespeace.Dtos;
using Portal.Dto;
using Abp.Application.Services.Dto;
using Portal.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using Portal.Storage;

namespace Portal.OrderItemNamespeace
{
    [AbpAuthorize(AppPermissions.Pages_OrderItems)]
    public class OrderItemsAppService : PortalAppServiceBase, IOrderItemsAppService
    {
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly IOrderItemsExcelExporter _orderItemsExcelExporter;

        public OrderItemsAppService(IRepository<OrderItem> orderItemRepository, IOrderItemsExcelExporter orderItemsExcelExporter)
        {
            _orderItemRepository = orderItemRepository;
            _orderItemsExcelExporter = orderItemsExcelExporter;

        }

        public virtual async Task<PagedResultDto<GetOrderItemForViewDto>> GetAll(GetAllOrderItemsInput input)
        {

            var filteredOrderItems = _orderItemRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinOrderIdFilter != null, e => e.OrderId >= input.MinOrderIdFilter)
                        .WhereIf(input.MaxOrderIdFilter != null, e => e.OrderId <= input.MaxOrderIdFilter)
                        .WhereIf(input.MinProductIdFilter != null, e => e.ProductId >= input.MinProductIdFilter)
                        .WhereIf(input.MaxProductIdFilter != null, e => e.ProductId <= input.MaxProductIdFilter)
                        .WhereIf(input.MinQuantityFilter != null, e => e.Quantity >= input.MinQuantityFilter)
                        .WhereIf(input.MaxQuantityFilter != null, e => e.Quantity <= input.MaxQuantityFilter)
                        .WhereIf(input.MinUnitPriceInclTaxFilter != null, e => e.UnitPriceInclTax >= input.MinUnitPriceInclTaxFilter)
                        .WhereIf(input.MaxUnitPriceInclTaxFilter != null, e => e.UnitPriceInclTax <= input.MaxUnitPriceInclTaxFilter)
                        .WhereIf(input.MinUnitPriceExclTaxFilter != null, e => e.UnitPriceExclTax >= input.MinUnitPriceExclTaxFilter)
                        .WhereIf(input.MaxUnitPriceExclTaxFilter != null, e => e.UnitPriceExclTax <= input.MaxUnitPriceExclTaxFilter)
                        .WhereIf(input.MinItemWeightFilter != null, e => e.ItemWeight >= input.MinItemWeightFilter)
                        .WhereIf(input.MaxItemWeightFilter != null, e => e.ItemWeight <= input.MaxItemWeightFilter)
                        .WhereIf(input.MinCreatedOnUtcFilter != null, e => e.CreatedOnUtc >= input.MinCreatedOnUtcFilter)
                        .WhereIf(input.MaxCreatedOnUtcFilter != null, e => e.CreatedOnUtc <= input.MaxCreatedOnUtcFilter)
                        .WhereIf(input.MinUpdatedOnUtcFilter != null, e => e.UpdatedOnUtc >= input.MinUpdatedOnUtcFilter)
                        .WhereIf(input.MaxUpdatedOnUtcFilter != null, e => e.UpdatedOnUtc <= input.MaxUpdatedOnUtcFilter);

            var pagedAndFilteredOrderItems = filteredOrderItems
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var orderItems = from o in pagedAndFilteredOrderItems
                             select new
                             {

                                 o.OrderId,
                                 o.ProductId,
                                 o.Quantity,
                                 o.UnitPriceInclTax,
                                 o.UnitPriceExclTax,
                                 o.ItemWeight,
                                 o.CreatedOnUtc,
                                 o.UpdatedOnUtc,
                                 Id = o.Id
                             };

            var totalCount = await filteredOrderItems.CountAsync();

            var dbList = await orderItems.ToListAsync();
            var results = new List<GetOrderItemForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOrderItemForViewDto()
                {
                    OrderItem = new OrderItemDto
                    {

                        OrderId = o.OrderId,
                        ProductId = o.ProductId,
                        Quantity = o.Quantity,
                        UnitPriceInclTax = o.UnitPriceInclTax,
                        UnitPriceExclTax = o.UnitPriceExclTax,
                        ItemWeight = o.ItemWeight,
                        CreatedOnUtc = o.CreatedOnUtc,
                        UpdatedOnUtc = o.UpdatedOnUtc,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOrderItemForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetOrderItemForViewDto> GetOrderItemForView(int id)
        {
            var orderItem = await _orderItemRepository.GetAsync(id);

            var output = new GetOrderItemForViewDto { OrderItem = ObjectMapper.Map<OrderItemDto>(orderItem) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_OrderItems_Edit)]
        public virtual async Task<GetOrderItemForEditOutput> GetOrderItemForEdit(EntityDto input)
        {
            var orderItem = await _orderItemRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOrderItemForEditOutput { OrderItem = ObjectMapper.Map<CreateOrEditOrderItemDto>(orderItem) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditOrderItemDto input)
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

        [AbpAuthorize(AppPermissions.Pages_OrderItems_Create)]
        protected virtual async Task Create(CreateOrEditOrderItemDto input)
        {
            var orderItem = ObjectMapper.Map<OrderItem>(input);

            await _orderItemRepository.InsertAsync(orderItem);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderItems_Edit)]
        protected virtual async Task Update(CreateOrEditOrderItemDto input)
        {
            var orderItem = await _orderItemRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, orderItem);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderItems_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _orderItemRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetOrderItemsToExcel(GetAllOrderItemsForExcelInput input)
        {

            var filteredOrderItems = _orderItemRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinOrderIdFilter != null, e => e.OrderId >= input.MinOrderIdFilter)
                        .WhereIf(input.MaxOrderIdFilter != null, e => e.OrderId <= input.MaxOrderIdFilter)
                        .WhereIf(input.MinProductIdFilter != null, e => e.ProductId >= input.MinProductIdFilter)
                        .WhereIf(input.MaxProductIdFilter != null, e => e.ProductId <= input.MaxProductIdFilter)
                        .WhereIf(input.MinQuantityFilter != null, e => e.Quantity >= input.MinQuantityFilter)
                        .WhereIf(input.MaxQuantityFilter != null, e => e.Quantity <= input.MaxQuantityFilter)
                        .WhereIf(input.MinUnitPriceInclTaxFilter != null, e => e.UnitPriceInclTax >= input.MinUnitPriceInclTaxFilter)
                        .WhereIf(input.MaxUnitPriceInclTaxFilter != null, e => e.UnitPriceInclTax <= input.MaxUnitPriceInclTaxFilter)
                        .WhereIf(input.MinUnitPriceExclTaxFilter != null, e => e.UnitPriceExclTax >= input.MinUnitPriceExclTaxFilter)
                        .WhereIf(input.MaxUnitPriceExclTaxFilter != null, e => e.UnitPriceExclTax <= input.MaxUnitPriceExclTaxFilter)
                        .WhereIf(input.MinItemWeightFilter != null, e => e.ItemWeight >= input.MinItemWeightFilter)
                        .WhereIf(input.MaxItemWeightFilter != null, e => e.ItemWeight <= input.MaxItemWeightFilter)
                        .WhereIf(input.MinCreatedOnUtcFilter != null, e => e.CreatedOnUtc >= input.MinCreatedOnUtcFilter)
                        .WhereIf(input.MaxCreatedOnUtcFilter != null, e => e.CreatedOnUtc <= input.MaxCreatedOnUtcFilter)
                        .WhereIf(input.MinUpdatedOnUtcFilter != null, e => e.UpdatedOnUtc >= input.MinUpdatedOnUtcFilter)
                        .WhereIf(input.MaxUpdatedOnUtcFilter != null, e => e.UpdatedOnUtc <= input.MaxUpdatedOnUtcFilter);

            var query = (from o in filteredOrderItems
                         select new GetOrderItemForViewDto()
                         {
                             OrderItem = new OrderItemDto
                             {
                                 OrderId = o.OrderId,
                                 ProductId = o.ProductId,
                                 Quantity = o.Quantity,
                                 UnitPriceInclTax = o.UnitPriceInclTax,
                                 UnitPriceExclTax = o.UnitPriceExclTax,
                                 ItemWeight = o.ItemWeight,
                                 CreatedOnUtc = o.CreatedOnUtc,
                                 UpdatedOnUtc = o.UpdatedOnUtc,
                                 Id = o.Id
                             }
                         });

            var orderItemListDtos = await query.ToListAsync();

            return _orderItemsExcelExporter.ExportToFile(orderItemListDtos);
        }

    }
}