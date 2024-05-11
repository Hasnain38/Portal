(function () {
  $(function () {
    var _$orderItemsTable = $('#OrderItemsTable');
    var _orderItemsService = abp.services.app.orderItems;

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
        getOrderItems();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getOrderItems();
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
        getOrderItems();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getOrderItems();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.OrderItems.Create'),
      edit: abp.auth.hasPermission('Pages.OrderItems.Edit'),
      delete: abp.auth.hasPermission('Pages.OrderItems.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/OrderItems/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/OrderItems/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditOrderItemModal',
    });

    var _viewOrderItemModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/OrderItems/VieworderItemModal',
      modalClass: 'ViewOrderItemModal',
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

    var dataTable = _$orderItemsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _orderItemsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#OrderItemsTableFilter').val(),
            minOrderIdFilter: $('#MinOrderIdFilterId').val(),
            maxOrderIdFilter: $('#MaxOrderIdFilterId').val(),
            minProductIdFilter: $('#MinProductIdFilterId').val(),
            maxProductIdFilter: $('#MaxProductIdFilterId').val(),
            minQuantityFilter: $('#MinQuantityFilterId').val(),
            maxQuantityFilter: $('#MaxQuantityFilterId').val(),
            minUnitPriceInclTaxFilter: $('#MinUnitPriceInclTaxFilterId').val(),
            maxUnitPriceInclTaxFilter: $('#MaxUnitPriceInclTaxFilterId').val(),
            minUnitPriceExclTaxFilter: $('#MinUnitPriceExclTaxFilterId').val(),
            maxUnitPriceExclTaxFilter: $('#MaxUnitPriceExclTaxFilterId').val(),
            minItemWeightFilter: $('#MinItemWeightFilterId').val(),
            maxItemWeightFilter: $('#MaxItemWeightFilterId').val(),
            minCreatedOnUtcFilter: getDateFilter($('#MinCreatedOnUtcFilterId')),
            maxCreatedOnUtcFilter: getMaxDateFilter($('#MaxCreatedOnUtcFilterId')),
            minUpdatedOnUtcFilter: getDateFilter($('#MinUpdatedOnUtcFilterId')),
            maxUpdatedOnUtcFilter: getMaxDateFilter($('#MaxUpdatedOnUtcFilterId')),
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
                  _viewOrderItemModal.open({ id: data.record.orderItem.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.orderItem.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteOrderItem(data.record.orderItem);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'orderItem.orderId',
          name: 'orderId',
        },
        {
          targets: 3,
          data: 'orderItem.productId',
          name: 'productId',
        },
        {
          targets: 4,
          data: 'orderItem.quantity',
          name: 'quantity',
        },
        {
          targets: 5,
          data: 'orderItem.unitPriceInclTax',
          name: 'unitPriceInclTax',
        },
        {
          targets: 6,
          data: 'orderItem.unitPriceExclTax',
          name: 'unitPriceExclTax',
        },
        {
          targets: 7,
          data: 'orderItem.itemWeight',
          name: 'itemWeight',
        },
        {
          targets: 8,
          data: 'orderItem.createdOnUtc',
          name: 'createdOnUtc',
          render: function (createdOnUtc) {
            if (createdOnUtc) {
              return moment(createdOnUtc).format('L');
            }
            return '';
          },
        },
        {
          targets: 9,
          data: 'orderItem.updatedOnUtc',
          name: 'updatedOnUtc',
          render: function (updatedOnUtc) {
            if (updatedOnUtc) {
              return moment(updatedOnUtc).format('L');
            }
            return '';
          },
        },
      ],
    });

    function getOrderItems() {
      dataTable.ajax.reload();
    }

    function deleteOrderItem(orderItem) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _orderItemsService
            .delete({
              id: orderItem.id,
            })
            .done(function () {
              getOrderItems(true);
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

    $('#CreateNewOrderItemButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _orderItemsService
        .getOrderItemsToExcel({
          filter: $('#OrderItemsTableFilter').val(),
          minOrderIdFilter: $('#MinOrderIdFilterId').val(),
          maxOrderIdFilter: $('#MaxOrderIdFilterId').val(),
          minProductIdFilter: $('#MinProductIdFilterId').val(),
          maxProductIdFilter: $('#MaxProductIdFilterId').val(),
          minQuantityFilter: $('#MinQuantityFilterId').val(),
          maxQuantityFilter: $('#MaxQuantityFilterId').val(),
          minUnitPriceInclTaxFilter: $('#MinUnitPriceInclTaxFilterId').val(),
          maxUnitPriceInclTaxFilter: $('#MaxUnitPriceInclTaxFilterId').val(),
          minUnitPriceExclTaxFilter: $('#MinUnitPriceExclTaxFilterId').val(),
          maxUnitPriceExclTaxFilter: $('#MaxUnitPriceExclTaxFilterId').val(),
          minItemWeightFilter: $('#MinItemWeightFilterId').val(),
          maxItemWeightFilter: $('#MaxItemWeightFilterId').val(),
          minCreatedOnUtcFilter: getDateFilter($('#MinCreatedOnUtcFilterId')),
          maxCreatedOnUtcFilter: getMaxDateFilter($('#MaxCreatedOnUtcFilterId')),
          minUpdatedOnUtcFilter: getDateFilter($('#MinUpdatedOnUtcFilterId')),
          maxUpdatedOnUtcFilter: getMaxDateFilter($('#MaxUpdatedOnUtcFilterId')),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    $('#ImportToExcelButton')
      .fileupload({
        url: abp.appPath + 'App/OrderItems/ImportFromExcel',
        dataType: 'json',
        maxFileSize: 1048576 * 100,
        dropZone: $('#OrderItemsTable'),
        done: function (e, response) {
          var jsonResult = response.result;
          if (jsonResult.success) {
            abp.notify.info(app.localize('ImportOrderItemsProcessStart'));
          } else {
            abp.notify.warn(app.localize('ImportOrderItemsUploadFailed'));
          }
        },
      })
      .prop('disabled', !$.support.fileInput)
      .parent()
      .addClass($.support.fileInput ? undefined : 'disabled');

    abp.event.on('app.createOrEditOrderItemModalSaved', function () {
      getOrderItems();
    });

    $('#GetOrderItemsButton').click(function (e) {
      e.preventDefault();
      getOrderItems();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getOrderItems();
      }
    });

    $('.reload-on-change').change(function (e) {
      getOrderItems();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getOrderItems();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getOrderItems();
    });
  });
})();
