(function ($) {
  app.modals.CreateOrEditRiskFunctionModal = function () {
    var _riskFunctionsService = abp.services.app.riskFunctions;

    var _modalManager;
    var _$riskFunctionInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$riskFunctionInformationForm = _modalManager.getModal().find('form[name=RiskFunctionInformationsForm]');
      _$riskFunctionInformationForm.validate();
    };

    this.save = function () {
      if (!_$riskFunctionInformationForm.valid()) {
        return;
      }

      var riskFunction = _$riskFunctionInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _riskFunctionsService
        .createOrEdit(riskFunction)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditRiskFunctionModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
