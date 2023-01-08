(function ($) {
    var $ = jQuery
  app.modals.CreateOrEditRiskStatusModal = function () {
    var _riskStatusService = abp.services.app.riskStatus;

    var _modalManager;
    var _$riskStatusInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      //modal.find('.date-picker').datetimepicker({
      //  locale: abp.localization.currentLanguage.name,
      //  format: 'L',
      //});

      _$riskStatusInformationForm = _modalManager.getModal().find('form[name=RiskStatusInformationsForm]');
      _$riskStatusInformationForm.validate();
    };

    this.save = function () {
      if (!_$riskStatusInformationForm.valid()) {
        return;
      }

      var riskStatus = _$riskStatusInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _riskStatusService
        .createOrEdit(riskStatus)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditRiskStatusModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
