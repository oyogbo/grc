(function ($) {
  app.modals.CreateOrEditReviewLevelModal = function () {
    var _reviewLevelsService = abp.services.app.reviewLevels;

    var _modalManager;
    var _$reviewLevelInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$reviewLevelInformationForm = _modalManager.getModal().find('form[name=ReviewLevelInformationsForm]');
      _$reviewLevelInformationForm.validate();
    };

    this.save = function () {
      if (!_$reviewLevelInformationForm.valid()) {
        return;
      }

      var reviewLevel = _$reviewLevelInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _reviewLevelsService
        .createOrEdit(reviewLevel)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditReviewLevelModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
