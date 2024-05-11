(function () {
  $(function () {
    var _$ordersTable = $('#OrdersTable');
    var _ordersService = abp.services.app.orders;

    var $selectedDate = {
      startDate: null,
      endDate: null,
    };

    $('.date-picker').on('apply.daterangepicker', function (ev, picker) {
      $(this).val(picker.startDate.format('MM/DD/YYYY'));
    });

    $('.startDate')
      .daterangepicker({
        autoUpdateInput: false,
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      })
      .on('apply.daterangepicker', (ev, picker) => {
        $selectedDate.startDate = picker.startDate;
        getOrders();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getOrders();
      });

    $('.endDate')
      .daterangepicker({
        autoUpdateInput: false,
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      })
      .on('apply.daterangepicker', (ev, picker) => {
        $selectedDate.endDate = picker.startDate;
        getOrders();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getOrders();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Orders.Create'),
      edit: abp.auth.hasPermission('Pages.Orders.Edit'),
      delete: abp.auth.hasPermission('Pages.Orders.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Orders/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Orders/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditOrderModal',
    });

    var _viewOrderModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Orders/VieworderModal',
      modalClass: 'ViewOrderModal',
    });

    var getDateFilter = function (element) {
      if ($selectedDate.startDate == null) {
        return null;
      }
      return $selectedDate.startDate.format('YYYY-MM-DDT00:00:00Z');
    };

    var getMaxDateFilter = function (element) {
      if ($selectedDate.endDate == null) {
        return null;
      }
      return $selectedDate.endDate.format('YYYY-MM-DDT23:59:59Z');
    };

    var dataTable = _$ordersTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _ordersService.getAll,
        inputFilter: function () {
          return {
            filter: $('#OrdersTableFilter').val(),
            orderGuidFilter: $('#OrderGuidFilterId').val(),
            minStoreIdFilter: $('#MinStoreIdFilterId').val(),
            maxStoreIdFilter: $('#MaxStoreIdFilterId').val(),
            minCustomerIdFilter: $('#MinCustomerIdFilterId').val(),
            maxCustomerIdFilter: $('#MaxCustomerIdFilterId').val(),
            minBillingAddressIdFilter: $('#MinBillingAddressIdFilterId').val(),
            maxBillingAddressIdFilter: $('#MaxBillingAddressIdFilterId').val(),
            minShippingAddressIdFilter: $('#MinShippingAddressIdFilterId').val(),
            maxShippingAddressIdFilter: $('#MaxShippingAddressIdFilterId').val(),
            minPickupAddressIdFilter: $('#MinPickupAddressIdFilterId').val(),
            maxPickupAddressIdFilter: $('#MaxPickupAddressIdFilterId').val(),
            pickupInStoreFilter: $('#PickupInStoreFilterId').val(),
            orderStatusIdFilter: $('#OrderStatusIdFilterId').val(),
            shippingStatusIdFilter: $('#ShippingStatusIdFilterId').val(),
            paymentStatusIdFilter: $('#PaymentStatusIdFilterId').val(),
            minOrderTaxFilter: $('#MinOrderTaxFilterId').val(),
            maxOrderTaxFilter: $('#MaxOrderTaxFilterId').val(),
            minOrderDiscountFilter: $('#MinOrderDiscountFilterId').val(),
            maxOrderDiscountFilter: $('#MaxOrderDiscountFilterId').val(),
            minOrderTotalFilter: $('#MinOrderTotalFilterId').val(),
            maxOrderTotalFilter: $('#MaxOrderTotalFilterId').val(),
            minCreatedOnUtcFilter: getDateFilter($('#MinCreatedOnUtcFilterId')),
            maxCreatedOnUtcFilter: getMaxDateFilter($('#MaxCreatedOnUtcFilterId')),
            minUpdatedOnUtcFilter: getDateFilter($('#MinUpdatedOnUtcFilterId')),
            maxUpdatedOnUtcFilter: getMaxDateFilter($('#MaxUpdatedOnUtcFilterId')),
            cardTypeIdFilter: $('#CardTypeIdFilterId').val(),
            cardNameFilter: $('#CardNameFilterId').val(),
            cardNumberFilter: $('#CardNumberFilterId').val(),
            deletedFilter: $('#DeletedFilterId').val(),
          };
        },
      },
      columnDefs: [
        {
          className: 'control responsive',
          orderable: false,
          render: function () {
            return '';
          },
          targets: 0,
        },
        {
          width: 120,
          targets: 1,
          data: null,
          orderable: false,
          autoWidth: false,
          defaultContent: '',
          rowAction: {
            cssClass: 'btn btn-brand dropdown-toggle',
            text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
            items: [
              {
                text: app.localize('View'),
                action: function (data) {
                  _viewOrderModal.open({ id: data.record.order.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.order.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteOrder(data.record.order);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'order.orderGuid',
          name: 'orderGuid',
        },
        {
          targets: 3,
          data: 'order.storeId',
          name: 'storeId',
        },
        {
          targets: 4,
          data: 'order.customerId',
          name: 'customerId',
        },
        {
          targets: 5,
          data: 'order.billingAddressId',
          name: 'billingAddressId',
        },
        {
          targets: 6,
          data: 'order.shippingAddressId',
          name: 'shippingAddressId',
        },
        {
          targets: 7,
          data: 'order.pickupAddressId',
          name: 'pickupAddressId',
        },
        {
          targets: 8,
          data: 'order.pickupInStore',
          name: 'pickupInStore',
          render: function (pickupInStore) {
            if (pickupInStore) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 9,
          data: 'order.orderStatusId',
          name: 'orderStatusId',
          render: function (orderStatusId) {
            return app.localize('Enum_OrderStatus_' + orderStatusId);
          },
        },
        {
          targets: 10,
          data: 'order.shippingStatusId',
          name: 'shippingStatusId',
          render: function (shippingStatusId) {
            return app.localize('Enum_ShippingStatus_' + shippingStatusId);
          },
        },
        {
          targets: 11,
          data: 'order.paymentStatusId',
          name: 'paymentStatusId',
          render: function (paymentStatusId) {
            return app.localize('Enum_PaymentStatus_' + paymentStatusId);
          },
        },
        {
          targets: 12,
          data: 'order.orderTax',
          name: 'orderTax',
        },
        {
          targets: 13,
          data: 'order.orderDiscount',
          name: 'orderDiscount',
        },
        {
          targets: 14,
          data: 'order.orderTotal',
          name: 'orderTotal',
        },
        {
          targets: 15,
          data: 'order.createdOnUtc',
          name: 'createdOnUtc',
          render: function (createdOnUtc) {
            if (createdOnUtc) {
              return moment(createdOnUtc).format('L');
            }
            return '';
          },
        },
        {
          targets: 16,
          data: 'order.updatedOnUtc',
          name: 'updatedOnUtc',
          render: function (updatedOnUtc) {
            if (updatedOnUtc) {
              return moment(updatedOnUtc).format('L');
            }
            return '';
          },
        },
        {
          targets: 17,
          data: 'order.cardTypeId',
          name: 'cardTypeId',
          render: function (cardTypeId) {
            return app.localize('Enum_CardType_' + cardTypeId);
          },
        },
        {
          targets: 18,
          data: 'order.cardName',
          name: 'cardName',
        },
        {
          targets: 19,
          data: 'order.cardNumber',
          name: 'cardNumber',
        },
        {
          targets: 20,
          data: 'order.deleted',
          name: 'deleted',
          render: function (deleted) {
            if (deleted) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
      ],
    });

    function getOrders() {
      dataTable.ajax.reload();
    }

    function deleteOrder(order) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _ordersService
            .delete({
              id: order.id,
            })
            .done(function () {
              getOrders(true);
              abp.notify.success(app.localize('SuccessfullyDeleted'));
            });
        }
      });
    }

    $('#ShowAdvancedFiltersSpan').click(function () {
      $('#ShowAdvancedFiltersSpan').hide();
      $('#HideAdvancedFiltersSpan').show();
      $('#AdvacedAuditFiltersArea').slideDown();
    });

    $('#HideAdvancedFiltersSpan').click(function () {
      $('#HideAdvancedFiltersSpan').hide();
      $('#ShowAdvancedFiltersSpan').show();
      $('#AdvacedAuditFiltersArea').slideUp();
    });

    $('#CreateNewOrderButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _ordersService
        .getOrdersToExcel({
          filter: $('#OrdersTableFilter').val(),
          orderGuidFilter: $('#OrderGuidFilterId').val(),
          minStoreIdFilter: $('#MinStoreIdFilterId').val(),
          maxStoreIdFilter: $('#MaxStoreIdFilterId').val(),
          minCustomerIdFilter: $('#MinCustomerIdFilterId').val(),
          maxCustomerIdFilter: $('#MaxCustomerIdFilterId').val(),
          minBillingAddressIdFilter: $('#MinBillingAddressIdFilterId').val(),
          maxBillingAddressIdFilter: $('#MaxBillingAddressIdFilterId').val(),
          minShippingAddressIdFilter: $('#MinShippingAddressIdFilterId').val(),
          maxShippingAddressIdFilter: $('#MaxShippingAddressIdFilterId').val(),
          minPickupAddressIdFilter: $('#MinPickupAddressIdFilterId').val(),
          maxPickupAddressIdFilter: $('#MaxPickupAddressIdFilterId').val(),
          pickupInStoreFilter: $('#PickupInStoreFilterId').val(),
          orderStatusIdFilter: $('#OrderStatusIdFilterId').val(),
          shippingStatusIdFilter: $('#ShippingStatusIdFilterId').val(),
          paymentStatusIdFilter: $('#PaymentStatusIdFilterId').val(),
          minOrderTaxFilter: $('#MinOrderTaxFilterId').val(),
          maxOrderTaxFilter: $('#MaxOrderTaxFilterId').val(),
          minOrderDiscountFilter: $('#MinOrderDiscountFilterId').val(),
          maxOrderDiscountFilter: $('#MaxOrderDiscountFilterId').val(),
          minOrderTotalFilter: $('#MinOrderTotalFilterId').val(),
          maxOrderTotalFilter: $('#MaxOrderTotalFilterId').val(),
          minCreatedOnUtcFilter: getDateFilter($('#MinCreatedOnUtcFilterId')),
          maxCreatedOnUtcFilter: getMaxDateFilter($('#MaxCreatedOnUtcFilterId')),
          minUpdatedOnUtcFilter: getDateFilter($('#MinUpdatedOnUtcFilterId')),
          maxUpdatedOnUtcFilter: getMaxDateFilter($('#MaxUpdatedOnUtcFilterId')),
          cardTypeIdFilter: $('#CardTypeIdFilterId').val(),
          cardNameFilter: $('#CardNameFilterId').val(),
          cardNumberFilter: $('#CardNumberFilterId').val(),
          deletedFilter: $('#DeletedFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    $('#ImportToExcelButton')
      .fileupload({
        url: abp.appPath + 'App/Orders/ImportFromExcel',
        dataType: 'json',
        maxFileSize: 1048576 * 100,
        dropZone: $('#OrdersTable'),
        done: function (e, response) {
          var jsonResult = response.result;
          if (jsonResult.success) {
            abp.notify.info(app.localize('ImportOrdersProcessStart'));
          } else {
            abp.notify.warn(app.localize('ImportOrdersUploadFailed'));
          }
        },
      })
      .prop('disabled', !$.support.fileInput)
      .parent()
      .addClass($.support.fileInput ? undefined : 'disabled');

    abp.event.on('app.createOrEditOrderModalSaved', function () {
      getOrders();
    });

    $('#GetOrdersButton').click(function (e) {
      e.preventDefault();
      getOrders();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getOrders();
      }
    });

    $('.reload-on-change').change(function (e) {
      getOrders();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getOrders();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getOrders();
    });
  });
})();
