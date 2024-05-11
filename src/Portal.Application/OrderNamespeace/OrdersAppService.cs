using Portal.GeneralEnums;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Portal.OrderNamespeace.Exporting;
using Portal.OrderNamespeace.Dtos;
using Portal.Dto;
using Abp.Application.Services.Dto;
using Portal.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using Portal.Storage;

namespace Portal.OrderNamespeace
{
    [AbpAuthorize(AppPermissions.Pages_Orders)]
    public class OrdersAppService : PortalAppServiceBase, IOrdersAppService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IOrdersExcelExporter _ordersExcelExporter;

        public OrdersAppService(IRepository<Order> orderRepository, IOrdersExcelExporter ordersExcelExporter)
        {
            _orderRepository = orderRepository;
            _ordersExcelExporter = ordersExcelExporter;

        }

        public virtual async Task<PagedResultDto<GetOrderForViewDto>> GetAll(GetAllOrdersInput input)
        {
            var orderStatusIdFilter = input.OrderStatusIdFilter.HasValue
                        ? (OrderStatus)input.OrderStatusIdFilter
                        : default;
            var shippingStatusIdFilter = input.ShippingStatusIdFilter.HasValue
                ? (ShippingStatus)input.ShippingStatusIdFilter
                : default;
            var paymentStatusIdFilter = input.PaymentStatusIdFilter.HasValue
                ? (PaymentStatus)input.PaymentStatusIdFilter
                : default;
            var cardTypeIdFilter = input.CardTypeIdFilter.HasValue
                ? (CardType)input.CardTypeIdFilter
                : default;

            var filteredOrders = _orderRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CardName.Contains(input.Filter) || e.CardNumber.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderGuidFilter.ToString()), e => e.OrderGuid.ToString() == input.OrderGuidFilter.ToString())
                        .WhereIf(input.MinStoreIdFilter != null, e => e.StoreId >= input.MinStoreIdFilter)
                        .WhereIf(input.MaxStoreIdFilter != null, e => e.StoreId <= input.MaxStoreIdFilter)
                        .WhereIf(input.MinCustomerIdFilter != null, e => e.CustomerId >= input.MinCustomerIdFilter)
                        .WhereIf(input.MaxCustomerIdFilter != null, e => e.CustomerId <= input.MaxCustomerIdFilter)
                        .WhereIf(input.MinBillingAddressIdFilter != null, e => e.BillingAddressId >= input.MinBillingAddressIdFilter)
                        .WhereIf(input.MaxBillingAddressIdFilter != null, e => e.BillingAddressId <= input.MaxBillingAddressIdFilter)
                        .WhereIf(input.MinShippingAddressIdFilter != null, e => e.ShippingAddressId >= input.MinShippingAddressIdFilter)
                        .WhereIf(input.MaxShippingAddressIdFilter != null, e => e.ShippingAddressId <= input.MaxShippingAddressIdFilter)
                        .WhereIf(input.MinPickupAddressIdFilter != null, e => e.PickupAddressId >= input.MinPickupAddressIdFilter)
                        .WhereIf(input.MaxPickupAddressIdFilter != null, e => e.PickupAddressId <= input.MaxPickupAddressIdFilter)
                        .WhereIf(input.PickupInStoreFilter.HasValue && input.PickupInStoreFilter > -1, e => (input.PickupInStoreFilter == 1 && e.PickupInStore) || (input.PickupInStoreFilter == 0 && !e.PickupInStore))
                        .WhereIf(input.OrderStatusIdFilter.HasValue && input.OrderStatusIdFilter > -1, e => e.OrderStatusId == orderStatusIdFilter)
                        .WhereIf(input.ShippingStatusIdFilter.HasValue && input.ShippingStatusIdFilter > -1, e => e.ShippingStatusId == shippingStatusIdFilter)
                        .WhereIf(input.PaymentStatusIdFilter.HasValue && input.PaymentStatusIdFilter > -1, e => e.PaymentStatusId == paymentStatusIdFilter)
                        .WhereIf(input.MinOrderTaxFilter != null, e => e.OrderTax >= input.MinOrderTaxFilter)
                        .WhereIf(input.MaxOrderTaxFilter != null, e => e.OrderTax <= input.MaxOrderTaxFilter)
                        .WhereIf(input.MinOrderDiscountFilter != null, e => e.OrderDiscount >= input.MinOrderDiscountFilter)
                        .WhereIf(input.MaxOrderDiscountFilter != null, e => e.OrderDiscount <= input.MaxOrderDiscountFilter)
                        .WhereIf(input.MinOrderTotalFilter != null, e => e.OrderTotal >= input.MinOrderTotalFilter)
                        .WhereIf(input.MaxOrderTotalFilter != null, e => e.OrderTotal <= input.MaxOrderTotalFilter)
                        .WhereIf(input.MinCreatedOnUtcFilter != null, e => e.CreatedOnUtc >= input.MinCreatedOnUtcFilter)
                        .WhereIf(input.MaxCreatedOnUtcFilter != null, e => e.CreatedOnUtc <= input.MaxCreatedOnUtcFilter)
                        .WhereIf(input.MinUpdatedOnUtcFilter != null, e => e.UpdatedOnUtc >= input.MinUpdatedOnUtcFilter)
                        .WhereIf(input.MaxUpdatedOnUtcFilter != null, e => e.UpdatedOnUtc <= input.MaxUpdatedOnUtcFilter)
                        .WhereIf(input.CardTypeIdFilter.HasValue && input.CardTypeIdFilter > -1, e => e.CardTypeId == cardTypeIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CardNameFilter), e => e.CardName.Contains(input.CardNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CardNumberFilter), e => e.CardNumber.Contains(input.CardNumberFilter))
                        .WhereIf(input.DeletedFilter.HasValue && input.DeletedFilter > -1, e => (input.DeletedFilter == 1 && e.Deleted) || (input.DeletedFilter == 0 && !e.Deleted));

            var pagedAndFilteredOrders = filteredOrders
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var orders = from o in pagedAndFilteredOrders
                         select new
                         {

                             o.OrderGuid,
                             o.StoreId,
                             o.CustomerId,
                             o.BillingAddressId,
                             o.ShippingAddressId,
                             o.PickupAddressId,
                             o.PickupInStore,
                             o.OrderStatusId,
                             o.ShippingStatusId,
                             o.PaymentStatusId,
                             o.OrderTax,
                             o.OrderDiscount,
                             o.OrderTotal,
                             o.CreatedOnUtc,
                             o.UpdatedOnUtc,
                             o.CardTypeId,
                             o.CardName,
                             o.CardNumber,
                             o.Deleted,
                             Id = o.Id
                         };

            var totalCount = await filteredOrders.CountAsync();

            var dbList = await orders.ToListAsync();
            var results = new List<GetOrderForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOrderForViewDto()
                {
                    Order = new OrderDto
                    {

                        OrderGuid = o.OrderGuid,
                        StoreId = o.StoreId,
                        CustomerId = o.CustomerId,
                        BillingAddressId = o.BillingAddressId,
                        ShippingAddressId = o.ShippingAddressId,
                        PickupAddressId = o.PickupAddressId,
                        PickupInStore = o.PickupInStore,
                        OrderStatusId = o.OrderStatusId,
                        ShippingStatusId = o.ShippingStatusId,
                        PaymentStatusId = o.PaymentStatusId,
                        OrderTax = o.OrderTax,
                        OrderDiscount = o.OrderDiscount,
                        OrderTotal = o.OrderTotal,
                        CreatedOnUtc = o.CreatedOnUtc,
                        UpdatedOnUtc = o.UpdatedOnUtc,
                        CardTypeId = o.CardTypeId,
                        CardName = o.CardName,
                        CardNumber = o.CardNumber,
                        Deleted = o.Deleted,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOrderForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetOrderForViewDto> GetOrderForView(int id)
        {
            var order = await _orderRepository.GetAsync(id);

            var output = new GetOrderForViewDto { Order = ObjectMapper.Map<OrderDto>(order) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Orders_Edit)]
        public virtual async Task<GetOrderForEditOutput> GetOrderForEdit(EntityDto input)
        {
            var order = await _orderRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOrderForEditOutput { Order = ObjectMapper.Map<CreateOrEditOrderDto>(order) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditOrderDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Orders_Create)]
        protected virtual async Task Create(CreateOrEditOrderDto input)
        {
            var order = ObjectMapper.Map<Order>(input);

            await _orderRepository.InsertAsync(order);

        }

        [AbpAuthorize(AppPermissions.Pages_Orders_Edit)]
        protected virtual async Task Update(CreateOrEditOrderDto input)
        {
            var order = await _orderRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, order);

        }

        [AbpAuthorize(AppPermissions.Pages_Orders_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _orderRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetOrdersToExcel(GetAllOrdersForExcelInput input)
        {
            var orderStatusIdFilter = input.OrderStatusIdFilter.HasValue
                        ? (OrderStatus)input.OrderStatusIdFilter
                        : default;
            var shippingStatusIdFilter = input.ShippingStatusIdFilter.HasValue
                ? (ShippingStatus)input.ShippingStatusIdFilter
                : default;
            var paymentStatusIdFilter = input.PaymentStatusIdFilter.HasValue
                ? (PaymentStatus)input.PaymentStatusIdFilter
                : default;
            var cardTypeIdFilter = input.CardTypeIdFilter.HasValue
                ? (CardType)input.CardTypeIdFilter
                : default;

            var filteredOrders = _orderRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CardName.Contains(input.Filter) || e.CardNumber.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderGuidFilter.ToString()), e => e.OrderGuid.ToString() == input.OrderGuidFilter.ToString())
                        .WhereIf(input.MinStoreIdFilter != null, e => e.StoreId >= input.MinStoreIdFilter)
                        .WhereIf(input.MaxStoreIdFilter != null, e => e.StoreId <= input.MaxStoreIdFilter)
                        .WhereIf(input.MinCustomerIdFilter != null, e => e.CustomerId >= input.MinCustomerIdFilter)
                        .WhereIf(input.MaxCustomerIdFilter != null, e => e.CustomerId <= input.MaxCustomerIdFilter)
                        .WhereIf(input.MinBillingAddressIdFilter != null, e => e.BillingAddressId >= input.MinBillingAddressIdFilter)
                        .WhereIf(input.MaxBillingAddressIdFilter != null, e => e.BillingAddressId <= input.MaxBillingAddressIdFilter)
                        .WhereIf(input.MinShippingAddressIdFilter != null, e => e.ShippingAddressId >= input.MinShippingAddressIdFilter)
                        .WhereIf(input.MaxShippingAddressIdFilter != null, e => e.ShippingAddressId <= input.MaxShippingAddressIdFilter)
                        .WhereIf(input.MinPickupAddressIdFilter != null, e => e.PickupAddressId >= input.MinPickupAddressIdFilter)
                        .WhereIf(input.MaxPickupAddressIdFilter != null, e => e.PickupAddressId <= input.MaxPickupAddressIdFilter)
                        .WhereIf(input.PickupInStoreFilter.HasValue && input.PickupInStoreFilter > -1, e => (input.PickupInStoreFilter == 1 && e.PickupInStore) || (input.PickupInStoreFilter == 0 && !e.PickupInStore))
                        .WhereIf(input.OrderStatusIdFilter.HasValue && input.OrderStatusIdFilter > -1, e => e.OrderStatusId == orderStatusIdFilter)
                        .WhereIf(input.ShippingStatusIdFilter.HasValue && input.ShippingStatusIdFilter > -1, e => e.ShippingStatusId == shippingStatusIdFilter)
                        .WhereIf(input.PaymentStatusIdFilter.HasValue && input.PaymentStatusIdFilter > -1, e => e.PaymentStatusId == paymentStatusIdFilter)
                        .WhereIf(input.MinOrderTaxFilter != null, e => e.OrderTax >= input.MinOrderTaxFilter)
                        .WhereIf(input.MaxOrderTaxFilter != null, e => e.OrderTax <= input.MaxOrderTaxFilter)
                        .WhereIf(input.MinOrderDiscountFilter != null, e => e.OrderDiscount >= input.MinOrderDiscountFilter)
                        .WhereIf(input.MaxOrderDiscountFilter != null, e => e.OrderDiscount <= input.MaxOrderDiscountFilter)
                        .WhereIf(input.MinOrderTotalFilter != null, e => e.OrderTotal >= input.MinOrderTotalFilter)
                        .WhereIf(input.MaxOrderTotalFilter != null, e => e.OrderTotal <= input.MaxOrderTotalFilter)
                        .WhereIf(input.MinCreatedOnUtcFilter != null, e => e.CreatedOnUtc >= input.MinCreatedOnUtcFilter)
                        .WhereIf(input.MaxCreatedOnUtcFilter != null, e => e.CreatedOnUtc <= input.MaxCreatedOnUtcFilter)
                        .WhereIf(input.MinUpdatedOnUtcFilter != null, e => e.UpdatedOnUtc >= input.MinUpdatedOnUtcFilter)
                        .WhereIf(input.MaxUpdatedOnUtcFilter != null, e => e.UpdatedOnUtc <= input.MaxUpdatedOnUtcFilter)
                        .WhereIf(input.CardTypeIdFilter.HasValue && input.CardTypeIdFilter > -1, e => e.CardTypeId == cardTypeIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CardNameFilter), e => e.CardName.Contains(input.CardNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CardNumberFilter), e => e.CardNumber.Contains(input.CardNumberFilter))
                        .WhereIf(input.DeletedFilter.HasValue && input.DeletedFilter > -1, e => (input.DeletedFilter == 1 && e.Deleted) || (input.DeletedFilter == 0 && !e.Deleted));

            var query = (from o in filteredOrders
                         select new GetOrderForViewDto()
                         {
                             Order = new OrderDto
                             {
                                 OrderGuid = o.OrderGuid,
                                 StoreId = o.StoreId,
                                 CustomerId = o.CustomerId,
                                 BillingAddressId = o.BillingAddressId,
                                 ShippingAddressId = o.ShippingAddressId,
                                 PickupAddressId = o.PickupAddressId,
                                 PickupInStore = o.PickupInStore,
                                 OrderStatusId = o.OrderStatusId,
                                 ShippingStatusId = o.ShippingStatusId,
                                 PaymentStatusId = o.PaymentStatusId,
                                 OrderTax = o.OrderTax,
                                 OrderDiscount = o.OrderDiscount,
                                 OrderTotal = o.OrderTotal,
                                 CreatedOnUtc = o.CreatedOnUtc,
                                 UpdatedOnUtc = o.UpdatedOnUtc,
                                 CardTypeId = o.CardTypeId,
                                 CardName = o.CardName,
                                 CardNumber = o.CardNumber,
                                 Deleted = o.Deleted,
                                 Id = o.Id
                             }
                         });

            var orderListDtos = await query.ToListAsync();

            return _ordersExcelExporter.ExportToFile(orderListDtos);
        }

    }
}