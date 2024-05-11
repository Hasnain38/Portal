(function () {
  $(function () {
    var _$systemDataDefinitionsTable = $('#SystemDataDefinitionsTable');
    var _systemDataDefinitionsService = abp.services.app.systemDataDefinitions;

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
        getSystemDataDefinitions();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getSystemDataDefinitions();
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
        getSystemDataDefinitions();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getSystemDataDefinitions();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.SystemDataDefinitions.Create'),
      edit: abp.auth.hasPermission('Pages.SystemDataDefinitions.Edit'),
      delete: abp.auth.hasPermission('Pages.SystemDataDefinitions.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/SystemDataDefinitions/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/SystemDataDefinitions/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditSystemDataDefinitionModal',
    });

    var _viewSystemDataDefinitionModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/SystemDataDefinitions/ViewsystemDataDefinitionModal',
      modalClass: 'ViewSystemDataDefinitionModal',
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

    var dataTable = _$systemDataDefinitionsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _systemDataDefinitionsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#SystemDataDefinitionsTableFilter').val(),
            minDefTypeIdFilter: $('#MinDefTypeIdFilterId').val(),
            maxDefTypeIdFilter: $('#MaxDefTypeIdFilterId').val(),
            defValueFilter: $('#DefValueFilterId').val(),
            minDefParentIdFilter: $('#MinDefParentIdFilterId').val(),
            maxDefParentIdFilter: $('#MaxDefParentIdFilterId').val(),
            minEntityIdFilter: $('#MinEntityIdFilterId').val(),
            maxEntityIdFilter: $('#MaxEntityIdFilterId').val(),
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
                  _viewSystemDataDefinitionModal.open({ id: data.record.systemDataDefinition.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.systemDataDefinition.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteSystemDataDefinition(data.record.systemDataDefinition);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'systemDataDefinition.defTypeId',
          name: 'defTypeId',
        },
        {
          targets: 3,
          data: 'systemDataDefinition.defValue',
          name: 'defValue',
        },
        {
          targets: 4,
          data: 'systemDataDefinition.defParentId',
          name: 'defParentId',
        },
        {
          targets: 5,
          data: 'systemDataDefinition.entityId',
          name: 'entityId',
        },
      ],
    });

    function getSystemDataDefinitions() {
      dataTable.ajax.reload();
    }

    function deleteSystemDataDefinition(systemDataDefinition) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _systemDataDefinitionsService
            .delete({
              id: systemDataDefinition.id,
            })
            .done(function () {
              getSystemDataDefinitions(true);
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

    $('#CreateNewSystemDataDefinitionButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _systemDataDefinitionsService
        .getSystemDataDefinitionsToExcel({
          filter: $('#SystemDataDefinitionsTableFilter').val(),
          minDefTypeIdFilter: $('#MinDefTypeIdFilterId').val(),
          maxDefTypeIdFilter: $('#MaxDefTypeIdFilterId').val(),
          defValueFilter: $('#DefValueFilterId').val(),
          minDefParentIdFilter: $('#MinDefParentIdFilterId').val(),
          maxDefParentIdFilter: $('#MaxDefParentIdFilterId').val(),
          minEntityIdFilter: $('#MinEntityIdFilterId').val(),
          maxEntityIdFilter: $('#MaxEntityIdFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    $('#ImportToExcelButton')
      .fileupload({
        url: abp.appPath + 'App/SystemDataDefinitions/ImportFromExcel',
        dataType: 'json',
        maxFileSize: 1048576 * 100,
        dropZone: $('#SystemDataDefinitionsTable'),
        done: function (e, response) {
          var jsonResult = response.result;
          if (jsonResult.success) {
            abp.notify.info(app.localize('ImportSystemDataDefinitionsProcessStart'));
          } else {
            abp.notify.warn(app.localize('ImportSystemDataDefinitionsUploadFailed'));
          }
        },
      })
      .prop('disabled', !$.support.fileInput)
      .parent()
      .addClass($.support.fileInput ? undefined : 'disabled');

    abp.event.on('app.createOrEditSystemDataDefinitionModalSaved', function () {
      getSystemDataDefinitions();
    });

    $('#GetSystemDataDefinitionsButton').click(function (e) {
      e.preventDefault();
      getSystemDataDefinitions();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getSystemDataDefinitions();
      }
    });

    $('.reload-on-change').change(function (e) {
      getSystemDataDefinitions();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getSystemDataDefinitions();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getSystemDataDefinitions();
    });
  });
})();
