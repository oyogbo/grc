(function ($) {
  app.modals.CreateOrEditEmployeesModal = function () {
    var _employeesService = abp.services.app.employees;

    var _modalManager;
    var _$employeesInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').datetimepicker({
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$employeesInformationForm = _modalManager.getModal().find('form[name=EmployeesInformationsForm]');
      _$employeesInformationForm.validate();
    };

    this.save = function () {
      if (!_$employeesInformationForm.valid()) {
        return;
      }

      var employees = _$employeesInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _employeesService
        .createOrEdit(employees)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditEmployeesModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
