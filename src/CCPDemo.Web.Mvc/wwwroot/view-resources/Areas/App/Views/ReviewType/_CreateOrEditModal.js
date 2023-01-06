(function ($) {
  app.modals.CreateOrEditReviewTypeModal = function () {
    var _reviewTypeService = abp.services.app.reviewType;

    var _modalManager;
    var _$reviewTypeInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').datetimepicker({
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$reviewTypeInformationForm = _modalManager.getModal().find('form[name=ReviewTypeInformationsForm]');
      _$reviewTypeInformationForm.validate();
    };

    this.save = function () {
      if (!_$reviewTypeInformationForm.valid()) {
        return;
      }

      var reviewType = _$reviewTypeInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _reviewTypeService
        .createOrEdit(reviewType)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditReviewTypeModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
