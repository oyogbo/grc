(function ($) {
  app.modals.CreateOrEditCloseReasonModal = function () {
    var _closeReasonsService = abp.services.app.closeReasons;

    var _modalManager;
    var _$closeReasonInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$closeReasonInformationForm = _modalManager.getModal().find('form[name=CloseReasonInformationsForm]');
      _$closeReasonInformationForm.validate();
    };

    this.save = function () {
      if (!_$closeReasonInformationForm.valid()) {
        return;
      }

      var closeReason = _$closeReasonInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _closeReasonsService
        .createOrEdit(closeReason)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditCloseReasonModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
