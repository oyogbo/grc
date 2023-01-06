(function ($) {
  app.modals.CreateOrEditManagementReviewModal = function () {
    var _managementReviewsService = abp.services.app.managementReviews;

    var _modalManager;
    var _$managementReviewInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').datetimepicker({
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$managementReviewInformationForm = _modalManager.getModal().find('form[name=ManagementReviewInformationsForm]');
      _$managementReviewInformationForm.validate();
    };

    this.save = function () {
      if (!_$managementReviewInformationForm.valid()) {
        return;
      }

      var managementReview = _$managementReviewInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _managementReviewsService
        .createOrEdit(managementReview)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditManagementReviewModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
