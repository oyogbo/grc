(function ($) {
  app.modals.CreateOrEditThreatCatalogModal = function () {
    var _threatCatalogService = abp.services.app.threatCatalog;

    var _modalManager;
    var _$threatCatalogInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$threatCatalogInformationForm = _modalManager.getModal().find('form[name=ThreatCatalogInformationsForm]');
      _$threatCatalogInformationForm.validate();
    };

    this.save = function () {
      if (!_$threatCatalogInformationForm.valid()) {
        return;
      }

      var threatCatalog = _$threatCatalogInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _threatCatalogService
        .createOrEdit(threatCatalog)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditThreatCatalogModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
