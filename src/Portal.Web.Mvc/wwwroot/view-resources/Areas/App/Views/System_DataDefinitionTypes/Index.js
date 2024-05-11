(function () {
  $(function () {
    var _$system_DataDefinitionTypesTable = $('#System_DataDefinitionTypesTable');
    var _system_DataDefinitionTypesService = abp.services.app.system_DataDefinitionTypes;

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
        getSystem_DataDefinitionTypes();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getSystem_DataDefinitionTypes();
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
        getSystem_DataDefinitionTypes();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getSystem_DataDefinitionTypes();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.System_DataDefinitionTypes.Create'),
      edit: abp.auth.hasPermission('Pages.System_DataDefinitionTypes.Edit'),
      delete: abp.auth.hasPermission('Pages.System_DataDefinitionTypes.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/System_DataDefinitionTypes/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/System_DataDefinitionTypes/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditSystem_DataDefinitionTypeModal',
    });

    var _viewSystem_DataDefinitionTypeModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/System_DataDefinitionTypes/Viewsystem_DataDefinitionTypeModal',
      modalClass: 'ViewSystem_DataDefinitionTypeModal',
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

    var dataTable = _$system_DataDefinitionTypesTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _system_DataDefinitionTypesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#System_DataDefinitionTypesTableFilter').val(),
            defTypeValueFilter: $('#DefTypeValueFilterId').val(),
            defTypeCodeFilter: $('#DefTypeCodeFilterId').val(),
            minDefTypeParentIdFilter: $('#MinDefTypeParentIdFilterId').val(),
            maxDefTypeParentIdFilter: $('#MaxDefTypeParentIdFilterId').val(),
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
                  _viewSystem_DataDefinitionTypeModal.open({ id: data.record.system_DataDefinitionType.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.system_DataDefinitionType.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteSystem_DataDefinitionType(data.record.system_DataDefinitionType);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'system_DataDefinitionType.defTypeValue',
          name: 'defTypeValue',
        },
        {
          targets: 3,
          data: 'system_DataDefinitionType.defTypeCode',
          name: 'defTypeCode',
        },
        {
          targets: 4,
          data: 'system_DataDefinitionType.defTypeParentId',
          name: 'defTypeParentId',
        },
      ],
    });

    function getSystem_DataDefinitionTypes() {
      dataTable.ajax.reload();
    }

    function deleteSystem_DataDefinitionType(system_DataDefinitionType) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _system_DataDefinitionTypesService
            .delete({
              id: system_DataDefinitionType.id,
            })
            .done(function () {
              getSystem_DataDefinitionTypes(true);
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

    $('#CreateNewSystem_DataDefinitionTypeButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _system_DataDefinitionTypesService
        .getSystem_DataDefinitionTypesToExcel({
          filter: $('#System_DataDefinitionTypesTableFilter').val(),
          defTypeValueFilter: $('#DefTypeValueFilterId').val(),
          defTypeCodeFilter: $('#DefTypeCodeFilterId').val(),
          minDefTypeParentIdFilter: $('#MinDefTypeParentIdFilterId').val(),
          maxDefTypeParentIdFilter: $('#MaxDefTypeParentIdFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    $('#ImportToExcelButton')
      .fileupload({
        url: abp.appPath + 'App/System_DataDefinitionTypes/ImportFromExcel',
        dataType: 'json',
        maxFileSize: 1048576 * 100,
        dropZone: $('#System_DataDefinitionTypesTable'),
        done: function (e, response) {
          var jsonResult = response.result;
          if (jsonResult.success) {
            abp.notify.info(app.localize('ImportSystem_DataDefinitionTypesProcessStart'));
          } else {
            abp.notify.warn(app.localize('ImportSystem_DataDefinitionTypesUploadFailed'));
          }
        },
      })
      .prop('disabled', !$.support.fileInput)
      .parent()
      .addClass($.support.fileInput ? undefined : 'disabled');

    abp.event.on('app.createOrEditSystem_DataDefinitionTypeModalSaved', function () {
      getSystem_DataDefinitionTypes();
    });

    $('#GetSystem_DataDefinitionTypesButton').click(function (e) {
      e.preventDefault();
      getSystem_DataDefinitionTypes();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getSystem_DataDefinitionTypes();
      }
    });

    $('.reload-on-change').change(function (e) {
      getSystem_DataDefinitionTypes();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getSystem_DataDefinitionTypes();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getSystem_DataDefinitionTypes();
    });
  });
})();
