(function ($) {
  app.modals.CreateOrEditSystem_DataDefinitionTypeModal = function () {
    var _system_DataDefinitionTypesService = abp.services.app.system_DataDefinitionTypes;

    var _modalManager;
    var _$system_DataDefinitionTypeInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$system_DataDefinitionTypeInformationForm = _modalManager
        .getModal()
        .find('form[name=System_DataDefinitionTypeInformationsForm]');
      _$system_DataDefinitionTypeInformationForm.validate();
    };

    this.save = function () {
      if (!_$system_DataDefinitionTypeInformationForm.valid()) {
        return;
      }

      var system_DataDefinitionType = _$system_DataDefinitionTypeInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _system_DataDefinitionTypesService
        .createOrEdit(system_DataDefinitionType)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditSystem_DataDefinitionTypeModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
