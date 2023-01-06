(function ($) {
  app.modals.CreateOrEditRiskCatalogModal = function () {
    var _riskCatalogsService = abp.services.app.riskCatalogs;

    var _modalManager;
    var _$riskCatalogInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$riskCatalogInformationForm = _modalManager.getModal().find('form[name=RiskCatalogInformationsForm]');
      _$riskCatalogInformationForm.validate();
    };

    this.save = function () {
      if (!_$riskCatalogInformationForm.valid()) {
        return;
      }

      var riskCatalog = _$riskCatalogInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _riskCatalogsService
        .createOrEdit(riskCatalog)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditRiskCatalogModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
