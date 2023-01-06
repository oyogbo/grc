(function ($) {
  app.modals.CreateOrEditThreatGroupingModal = function () {
    var _threatGroupingService = abp.services.app.threatGrouping;

    var _modalManager;
    var _$threatGroupingInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$threatGroupingInformationForm = _modalManager.getModal().find('form[name=ThreatGroupingInformationsForm]');
      _$threatGroupingInformationForm.validate();
    };

    this.save = function () {
      if (!_$threatGroupingInformationForm.valid()) {
        return;
      }

      var threatGrouping = _$threatGroupingInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _threatGroupingService
        .createOrEdit(threatGrouping)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditThreatGroupingModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
