(function ($) {
  app.modals.CreateOrEditRiskRatingModal = function () {
    var _riskRatingsService = abp.services.app.riskRatings;

    var _modalManager;
    var _$riskRatingInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      //modal.find('.date-picker').datetimepicker({
      //  locale: abp.localization.currentLanguage.name,
      //  format: 'L',
      //});

      _$riskRatingInformationForm = _modalManager.getModal().find('form[name=RiskRatingInformationsForm]');
      _$riskRatingInformationForm.validate();
    };

    this.save = function () {
      if (!_$riskRatingInformationForm.valid()) {
        return;
      }

      var riskRating = _$riskRatingInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _riskRatingsService
        .createOrEdit(riskRating)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditRiskRatingModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
