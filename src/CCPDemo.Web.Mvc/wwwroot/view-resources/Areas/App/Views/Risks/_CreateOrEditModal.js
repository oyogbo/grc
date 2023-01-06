(function ($) {
  app.modals.CreateOrEditRiskModal = function () {
    var _risksService = abp.services.app.risks;

    var _modalManager;
    var _$riskInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
        modal.find('.date-picker').flatpickr({
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$riskInformationForm = _modalManager.getModal().find('form[name=RiskInformationsForm]');
      _$riskInformationForm.validate();
    };

    this.save = function () {
      if (!_$riskInformationForm.valid()) {
        return;
      }

      var risk = _$riskInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _risksService
        .createOrEdit(risk)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditRiskModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };

  };
})(jQuery);
