(function () {
  $(function () {
    var _$testEntitiesTable = $('#TestEntitiesTable');
    var _testEntitiesService = abp.services.app.testEntities;

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
        getTestEntities();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getTestEntities();
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
        getTestEntities();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getTestEntities();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.TestEntities.Create'),
      edit: abp.auth.hasPermission('Pages.TestEntities.Edit'),
      delete: abp.auth.hasPermission('Pages.TestEntities.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/TestEntities/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/TestEntities/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditTestEntityModal',
    });

    var _viewTestEntityModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/TestEntities/ViewtestEntityModal',
      modalClass: 'ViewTestEntityModal',
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

    var dataTable = _$testEntitiesTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _testEntitiesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#TestEntitiesTableFilter').val(),
            testNameFilter: $('#TestNameFilterId').val(),
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
                  _viewTestEntityModal.open({ id: data.record.testEntity.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.testEntity.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteTestEntity(data.record.testEntity);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'testEntity.testName',
          name: 'testName',
        },
      ],
    });

    function getTestEntities() {
      dataTable.ajax.reload();
    }

    function deleteTestEntity(testEntity) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _testEntitiesService
            .delete({
              id: testEntity.id,
            })
            .done(function () {
              getTestEntities(true);
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

    $('#CreateNewTestEntityButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _testEntitiesService
        .getTestEntitiesToExcel({
          filter: $('#TestEntitiesTableFilter').val(),
          testNameFilter: $('#TestNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    $('#ImportToExcelButton')
      .fileupload({
        url: abp.appPath + 'App/TestEntities/ImportFromExcel',
        dataType: 'json',
        maxFileSize: 1048576 * 100,
        dropZone: $('#TestEntitiesTable'),
        done: function (e, response) {
          var jsonResult = response.result;
          if (jsonResult.success) {
            abp.notify.info(app.localize('ImportTestEntitiesProcessStart'));
          } else {
            abp.notify.warn(app.localize('ImportTestEntitiesUploadFailed'));
          }
        },
      })
      .prop('disabled', !$.support.fileInput)
      .parent()
      .addClass($.support.fileInput ? undefined : 'disabled');

    abp.event.on('app.createOrEditTestEntityModalSaved', function () {
      getTestEntities();
    });

    $('#GetTestEntitiesButton').click(function (e) {
      e.preventDefault();
      getTestEntities();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getTestEntities();
      }
    });

    $('.reload-on-change').change(function (e) {
      getTestEntities();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getTestEntities();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getTestEntities();
    });
  });
})();
