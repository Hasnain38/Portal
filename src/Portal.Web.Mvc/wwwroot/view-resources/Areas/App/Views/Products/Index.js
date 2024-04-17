(function () {
  $(function () {
    var _$productsTable = $('#ProductsTable');
    var _productsService = abp.services.app.products;

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
        getProducts();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getProducts();
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
        getProducts();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getProducts();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Products.Create'),
      edit: abp.auth.hasPermission('Pages.Products.Edit'),
      delete: abp.auth.hasPermission('Pages.Products.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Products/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Products/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditProductModal',
    });

    var _viewProductModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Products/ViewproductModal',
      modalClass: 'ViewProductModal',
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

    var dataTable = _$productsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _productsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#ProductsTableFilter').val(),
            productNameFilter: $('#ProductNameFilterId').val(),
            shortDescriptionFilter: $('#ShortDescriptionFilterId').val(),
            fullDescriptionFilter: $('#FullDescriptionFilterId').val(),
            minProductTemplateIdFilter: $('#MinProductTemplateIdFilterId').val(),
            maxProductTemplateIdFilter: $('#MaxProductTemplateIdFilterId').val(),
            minVendorIdFilter: $('#MinVendorIdFilterId').val(),
            maxVendorIdFilter: $('#MaxVendorIdFilterId').val(),
            showOnHomepageFilter: $('#ShowOnHomepageFilterId').val(),
            metaKeywordsFilter: $('#MetaKeywordsFilterId').val(),
            metaDescriptionFilter: $('#MetaDescriptionFilterId').val(),
            metaTitleFilter: $('#MetaTitleFilterId').val(),
            allowCustomerReviewsFilter: $('#AllowCustomerReviewsFilterId').val(),
            skuFilter: $('#SkuFilterId').val(),
            isGiftCardFilter: $('#IsGiftCardFilterId').val(),
            giftCardTypeIdFilter: $('#GiftCardTypeIdFilterId').val(),
            minWarehouseIdFilter: $('#MinWarehouseIdFilterId').val(),
            maxWarehouseIdFilter: $('#MaxWarehouseIdFilterId').val(),
            minCreatedOnUtcFilter: getDateFilter($('#MinCreatedOnUtcFilterId')),
            maxCreatedOnUtcFilter: getMaxDateFilter($('#MaxCreatedOnUtcFilterId')),
            minUpdatedOnUtcFilter: getDateFilter($('#MinUpdatedOnUtcFilterId')),
            maxUpdatedOnUtcFilter: getMaxDateFilter($('#MaxUpdatedOnUtcFilterId')),
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
                  _viewProductModal.open({ id: data.record.product.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.product.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteProduct(data.record.product);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'product.productName',
          name: 'productName',
        },
        {
          targets: 3,
          data: 'product.shortDescription',
          name: 'shortDescription',
        },
        {
          targets: 4,
          data: 'product.fullDescription',
          name: 'fullDescription',
        },
        {
          targets: 5,
          data: 'product.productTemplateId',
          name: 'productTemplateId',
        },
        {
          targets: 6,
          data: 'product.vendorId',
          name: 'vendorId',
        },
        {
          targets: 7,
          data: 'product.showOnHomepage',
          name: 'showOnHomepage',
          render: function (showOnHomepage) {
            if (showOnHomepage) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 8,
          data: 'product.metaKeywords',
          name: 'metaKeywords',
        },
        {
          targets: 9,
          data: 'product.metaDescription',
          name: 'metaDescription',
        },
        {
          targets: 10,
          data: 'product.metaTitle',
          name: 'metaTitle',
        },
        {
          targets: 11,
          data: 'product.allowCustomerReviews',
          name: 'allowCustomerReviews',
          render: function (allowCustomerReviews) {
            if (allowCustomerReviews) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 12,
          data: 'product.sku',
          name: 'sku',
        },
        {
          targets: 13,
          data: 'product.isGiftCard',
          name: 'isGiftCard',
          render: function (isGiftCard) {
            if (isGiftCard) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 14,
          data: 'product.giftCardTypeId',
          name: 'giftCardTypeId',
        },
        {
          targets: 15,
          data: 'product.warehouseId',
          name: 'warehouseId',
        },
        {
          targets: 16,
          data: 'product.createdOnUtc',
          name: 'createdOnUtc',
          render: function (createdOnUtc) {
            if (createdOnUtc) {
              return moment(createdOnUtc).format('L');
            }
            return '';
          },
        },
        {
          targets: 17,
          data: 'product.updatedOnUtc',
          name: 'updatedOnUtc',
          render: function (updatedOnUtc) {
            if (updatedOnUtc) {
              return moment(updatedOnUtc).format('L');
            }
            return '';
          },
        },
        {
          targets: 18,
          data: 'product.deleted',
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

    function getProducts() {
      dataTable.ajax.reload();
    }

    function deleteProduct(product) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _productsService
            .delete({
              id: product.id,
            })
            .done(function () {
              getProducts(true);
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

    $('#CreateNewProductButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _productsService
        .getProductsToExcel({
          filter: $('#ProductsTableFilter').val(),
          productNameFilter: $('#ProductNameFilterId').val(),
          shortDescriptionFilter: $('#ShortDescriptionFilterId').val(),
          fullDescriptionFilter: $('#FullDescriptionFilterId').val(),
          minProductTemplateIdFilter: $('#MinProductTemplateIdFilterId').val(),
          maxProductTemplateIdFilter: $('#MaxProductTemplateIdFilterId').val(),
          minVendorIdFilter: $('#MinVendorIdFilterId').val(),
          maxVendorIdFilter: $('#MaxVendorIdFilterId').val(),
          showOnHomepageFilter: $('#ShowOnHomepageFilterId').val(),
          metaKeywordsFilter: $('#MetaKeywordsFilterId').val(),
          metaDescriptionFilter: $('#MetaDescriptionFilterId').val(),
          metaTitleFilter: $('#MetaTitleFilterId').val(),
          allowCustomerReviewsFilter: $('#AllowCustomerReviewsFilterId').val(),
          skuFilter: $('#SkuFilterId').val(),
          isGiftCardFilter: $('#IsGiftCardFilterId').val(),
          giftCardTypeIdFilter: $('#GiftCardTypeIdFilterId').val(),
          minWarehouseIdFilter: $('#MinWarehouseIdFilterId').val(),
          maxWarehouseIdFilter: $('#MaxWarehouseIdFilterId').val(),
          minCreatedOnUtcFilter: getDateFilter($('#MinCreatedOnUtcFilterId')),
          maxCreatedOnUtcFilter: getMaxDateFilter($('#MaxCreatedOnUtcFilterId')),
          minUpdatedOnUtcFilter: getDateFilter($('#MinUpdatedOnUtcFilterId')),
          maxUpdatedOnUtcFilter: getMaxDateFilter($('#MaxUpdatedOnUtcFilterId')),
          deletedFilter: $('#DeletedFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    $('#ImportToExcelButton')
      .fileupload({
        url: abp.appPath + 'App/Products/ImportFromExcel',
        dataType: 'json',
        maxFileSize: 1048576 * 100,
        dropZone: $('#ProductsTable'),
        done: function (e, response) {
          var jsonResult = response.result;
          if (jsonResult.success) {
            abp.notify.info(app.localize('ImportProductsProcessStart'));
          } else {
            abp.notify.warn(app.localize('ImportProductsUploadFailed'));
          }
        },
      })
      .prop('disabled', !$.support.fileInput)
      .parent()
      .addClass($.support.fileInput ? undefined : 'disabled');

    abp.event.on('app.createOrEditProductModalSaved', function () {
      getProducts();
    });

    $('#GetProductsButton').click(function (e) {
      e.preventDefault();
      getProducts();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getProducts();
      }
    });

    $('.reload-on-change').change(function (e) {
      getProducts();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getProducts();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getProducts();
    });
  });
})();
