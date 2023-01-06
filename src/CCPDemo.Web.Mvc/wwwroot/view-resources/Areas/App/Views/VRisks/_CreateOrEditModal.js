(function ($) {
  app.modals.CreateOrEditVRiskModal = function () {
      var _vRisksService = abp.services.app.vRisks;

    var _modalManager;
    var _$vRiskInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      //var modal = _modalManager.getModal();
      //modal.find('.date-picker').datetimepicker({
      //  locale: abp.localization.currentLanguage.name,
      //  format: 'L',
      //});

      _$vRiskInformationForm = _modalManager.getModal().find('form[name=VRiskInformationsForm]');
      _$vRiskInformationForm.validate();
    };

    this.save = function () {
      if (!_$vRiskInformationForm.valid()) {
        return;
      }

      var vRisk = _$vRiskInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _vRisksService
          .createOrEdit(vRisk)
        .done(function () {
            abp.notify.info(app.localize('SavedSuccessfully'));
            //_vRisksService.sendEmail();

          _modalManager.close();
          abp.event.trigger('app.createOrEditVRiskModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };

    $("#VRisk_AcceptanceDate").flatpickr();
    $("#VRisk_ActualClosureDate").flatpickr();
    $("#VRisk_MitigationDate").flatpickr();
    $("#VRisk_ResolutionTimeLine").flatpickr();

    //$('input[name="mitigationDate"]').daterangepicker({
    //    timePicker: true,
    //    startDate: moment().startOf('hour'),
    //    endDate: moment().startOf('hour').add(32, 'hour'),
    //    locale: {
    //        format: 'M/DD hh:mm A'
    //    }
    //});
    //$('input[name="resolutionTimeLine"]').daterangepicker({
    //    timePicker: true,
    //    startDate: moment().startOf('hour'),
    //    endDate: moment().startOf('hour').add(32, 'hour'),
    //    locale: {
    //        format: 'M/DD hh:mm A'
    //    }
    //});

})(jQuery);
