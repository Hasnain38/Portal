(function ($) {
  app.modals.CreateOrEditProductModal = function () {
    var _productsService = abp.services.app.products;

    var _modalManager;
    var _$productInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$productInformationForm = _modalManager.getModal().find('form[name=ProductInformationsForm]');
      _$productInformationForm.validate();
    };

    this.save = function () {
      if (!_$productInformationForm.valid()) {
        return;
      }

      var product = _$productInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _productsService
        .createOrEdit(product)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditProductModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
