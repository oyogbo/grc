(function ($) {
  app.modals.CreateOrEditMitigationModal = function () {
    var _mitigationsService = abp.services.app.mitigations;

    var _modalManager;
    var _$mitigationInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').datetimepicker({
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$mitigationInformationForm = _modalManager.getModal().find('form[name=MitigationInformationsForm]');
      _$mitigationInformationForm.validate();
    };

    this.save = function () {
      if (!_$mitigationInformationForm.valid()) {
        return;
      }

      var mitigation = _$mitigationInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _mitigationsService
        .createOrEdit(mitigation)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditMitigationModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
