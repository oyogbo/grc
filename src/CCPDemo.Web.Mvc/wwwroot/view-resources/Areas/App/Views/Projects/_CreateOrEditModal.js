(function ($) {
  app.modals.CreateOrEditProjectModal = function () {
    var _projectsService = abp.services.app.projects;

    var _modalManager;
    var _$projectInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$projectInformationForm = _modalManager.getModal().find('form[name=ProjectInformationsForm]');
      _$projectInformationForm.validate();
    };

    this.save = function () {
      if (!_$projectInformationForm.valid()) {
        return;
      }

      var project = _$projectInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _projectsService
        .createOrEdit(project)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditProjectModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
