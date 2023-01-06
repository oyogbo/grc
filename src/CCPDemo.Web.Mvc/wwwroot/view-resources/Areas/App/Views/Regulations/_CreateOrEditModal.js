(function ($) {
  app.modals.CreateOrEditRegulationModal = function () {
    var _regulationsService = abp.services.app.regulations;

    var _modalManager;
    var _$regulationInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').datetimepicker({
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$regulationInformationForm = _modalManager.getModal().find('form[name=RegulationInformationsForm]');
      _$regulationInformationForm.validate();
    };

    this.save = function () {
      if (!_$regulationInformationForm.valid()) {
        return;
      }

      var regulation = _$regulationInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _regulationsService
        .createOrEdit(regulation)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditRegulationModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
