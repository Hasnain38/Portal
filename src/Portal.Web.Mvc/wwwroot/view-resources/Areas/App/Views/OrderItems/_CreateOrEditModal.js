(function ($) {
  app.modals.CreateOrEditOrderItemModal = function () {
    var _orderItemsService = abp.services.app.orderItems;

    var _modalManager;
    var _$orderItemInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$orderItemInformationForm = _modalManager.getModal().find('form[name=OrderItemInformationsForm]');
      _$orderItemInformationForm.validate();
    };

    this.save = function () {
      if (!_$orderItemInformationForm.valid()) {
        return;
      }

      var orderItem = _$orderItemInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _orderItemsService
        .createOrEdit(orderItem)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditOrderItemModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
