var $ = jQuery
(function ($) {
  app.modals.CreateOrEditStatusModal = function () {
    var _statusService = abp.services.app.status;

    var _modalManager;
    var _$statusInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
        modal.find('.date-picker').flatpickr({
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$statusInformationForm = _modalManager.getModal().find('form[name=StatusInformationsForm]');
      _$statusInformationForm.validate();
    };

    this.save = function () {
      if (!_$statusInformationForm.valid()) {
        return;
      }

      var status = _$statusInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _statusService
        .createOrEdit(status)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditStatusModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
