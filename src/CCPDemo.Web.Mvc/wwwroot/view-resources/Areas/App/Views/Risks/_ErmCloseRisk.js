(function ($) {


    $('#riskTypeId').attr("disabled", "disabled");
    $('#organizationUnitId').attr("disabled", "disabled");
    //$('#statusId').attr("disabled", "disabled");
    $('#riskRatingId').attr("disabled", "disabled");
    $('#userId').attr("disabled", "disabled");
    $('#Risk_Summary').attr("disabled", "disabled");
    $('#Risk_ExistingControl').attr("disabled", "disabled");
    $('#Risk_ERMRecommendation').attr("disabled", "disabled");
    $('#Risk_ActionPlan').attr("disabled", "disabled");
    $('#Risk_RiskOwnerComment').attr("disabled", "disabled");
    $('#Risk_TargetDate').attr("disabled", "disabled");
    $('#Risk_ActualClosureDate').attr("disabled", "disabled");
    $('#Risk_AcceptanceDate').attr("disabled", "disabled");
    $('#Risk_RiskAccepted').prop("disabled", "disabled");

  app.modals.ErmCloseRisk = function () {
    var _risksService = abp.services.app.risks;

    var _modalManager;
      var _$riskInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
        modal.find('.date-picker').datetimepicker({
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$riskInformationForm = _modalManager.getModal().find('form[name=RiskInformationsForm]');
      _$riskInformationForm.validate();
    };


    this.save = function () {
      if (!_$riskInformationForm.valid()) {
        return;
      }

      var risk = _$riskInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _risksService
        .createOrEdit(risk)
          .done(function (data) {
            console.log(data)
          abp.notify.info("Risk Successfully Closed!");
          _modalManager.close();
          abp.event.trigger('app.createOrEditRiskModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
