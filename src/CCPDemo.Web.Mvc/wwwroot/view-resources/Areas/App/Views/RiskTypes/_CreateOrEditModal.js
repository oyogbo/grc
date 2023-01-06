(function ($) {
  app.modals.CreateOrEditRiskTypeModal = function () {
    var _riskTypesService = abp.services.app.riskTypes;

    var _modalManager;
    var _$riskTypeInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      //modal.find('.date-picker').datetimepicker({
      //  locale: abp.localization.currentLanguage.name,
      //  format: 'L',
      //});

      _$riskTypeInformationForm = _modalManager.getModal().find('form[name=RiskTypeInformationsForm]');
      _$riskTypeInformationForm.validate();
    };

    this.save = function () {
      if (!_$riskTypeInformationForm.valid()) {
        return;
      }

      var riskType = _$riskTypeInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _riskTypesService
        .createOrEdit(riskType)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditRiskTypeModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
