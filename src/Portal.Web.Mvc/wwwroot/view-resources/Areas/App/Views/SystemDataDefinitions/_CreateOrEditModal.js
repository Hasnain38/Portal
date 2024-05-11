(function ($) {
  app.modals.CreateOrEditSystemDataDefinitionModal = function () {
    var _systemDataDefinitionsService = abp.services.app.systemDataDefinitions;

    var _modalManager;
    var _$systemDataDefinitionInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$systemDataDefinitionInformationForm = _modalManager
        .getModal()
        .find('form[name=SystemDataDefinitionInformationsForm]');
      _$systemDataDefinitionInformationForm.validate();
    };

    this.save = function () {
      if (!_$systemDataDefinitionInformationForm.valid()) {
        return;
      }

      var systemDataDefinition = _$systemDataDefinitionInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _systemDataDefinitionsService
        .createOrEdit(systemDataDefinition)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditSystemDataDefinitionModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
