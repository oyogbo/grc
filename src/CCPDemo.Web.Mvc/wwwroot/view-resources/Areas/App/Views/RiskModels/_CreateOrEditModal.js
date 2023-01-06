(function ($) {
  app.modals.CreateOrEditRiskModelModal = function () {
    var _riskModelsService = abp.services.app.riskModels;

    var _modalManager;
    var _$riskModelInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$riskModelInformationForm = _modalManager.getModal().find('form[name=RiskModelInformationsForm]');
      _$riskModelInformationForm.validate();
    };

    this.save = function () {
      if (!_$riskModelInformationForm.valid()) {
        return;
      }

      var riskModel = _$riskModelInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _riskModelsService
        .createOrEdit(riskModel)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditRiskModelModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
