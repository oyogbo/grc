(function ($) {
  app.modals.CreateOrEditClosureModal = function () {
    var _closuresService = abp.services.app.closures;

    var _modalManager;
    var _$closureInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$closureInformationForm = _modalManager.getModal().find('form[name=ClosureInformationsForm]');
      _$closureInformationForm.validate();
    };

    this.save = function () {
      if (!_$closureInformationForm.valid()) {
        return;
      }

      var closure = _$closureInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _closuresService
        .createOrEdit(closure)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditClosureModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
