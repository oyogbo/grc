(function ($) {
  app.modals.CreateOrEditDepartmentModal = function () {
    var _departmentsService = abp.services.app.departments;

    var _modalManager;
    var _$departmentInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      //modal.find('.date-picker').datetimepicker({
      //  locale: abp.localization.currentLanguage.name,
      //  format: 'L',
      //});

      _$departmentInformationForm = _modalManager.getModal().find('form[name=DepartmentInformationsForm]');
      _$departmentInformationForm.validate();
    };

    this.save = function () {
      if (!_$departmentInformationForm.valid()) {
        return;
      }

      var department = _$departmentInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _departmentsService
        .createOrEdit(department)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditDepartmentModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
