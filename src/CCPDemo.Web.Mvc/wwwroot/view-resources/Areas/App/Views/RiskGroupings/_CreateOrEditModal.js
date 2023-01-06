(function ($) {
  app.modals.CreateOrEditRiskGroupingModal = function () {
    var _riskGroupingsService = abp.services.app.riskGroupings;

    var _modalManager;
    var _$riskGroupingInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$riskGroupingInformationForm = _modalManager.getModal().find('form[name=RiskGroupingInformationsForm]');
      _$riskGroupingInformationForm.validate();
    };

    this.save = function () {
      if (!_$riskGroupingInformationForm.valid()) {
        return;
      }

      var riskGrouping = _$riskGroupingInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _riskGroupingsService
        .createOrEdit(riskGrouping)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditRiskGroupingModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
