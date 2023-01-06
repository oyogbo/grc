(function ($) {
  app.modals.CreateOrEditRiskLevelModal = function () {
    var _riskLevelsService = abp.services.app.riskLevels;

    var _modalManager;
    var _$riskLevelInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$riskLevelInformationForm = _modalManager.getModal().find('form[name=RiskLevelInformationsForm]');
      _$riskLevelInformationForm.validate();
    };

    this.save = function () {
      if (!_$riskLevelInformationForm.valid()) {
        return;
      }

      var riskLevel = _$riskLevelInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _riskLevelsService
        .createOrEdit(riskLevel)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditRiskLevelModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
