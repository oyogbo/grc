(function ($) {
  app.modals.CreateOrEditAssessmentModal = function () {
    var _assessmentsService = abp.services.app.assessments;

    var _modalManager;
    var _$assessmentInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$assessmentInformationForm = _modalManager.getModal().find('form[name=AssessmentInformationsForm]');
      _$assessmentInformationForm.validate();
    };

    this.save = function () {
      if (!_$assessmentInformationForm.valid()) {
        return;
      }

      var assessment = _$assessmentInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _assessmentsService
        .createOrEdit(assessment)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditAssessmentModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
