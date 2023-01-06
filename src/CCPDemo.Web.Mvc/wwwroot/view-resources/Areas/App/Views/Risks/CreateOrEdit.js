(function () {
  $(function () {
    var _risksService = abp.services.app.risks;

    var _$riskInformationForm = $('form[name=RiskInformationsForm]');
    _$riskInformationForm.validate();

      $('.date-picker').flatpickr({
      locale: abp.localization.currentLanguage.name,
      format: 'L',
    });

    function save(successCallback) {
      if (!_$riskInformationForm.valid()) {
        return;
      }

      var risk = _$riskInformationForm.serializeFormToObject();

      abp.ui.setBusy();
      _risksService
        .createOrEdit(risk)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          abp.event.trigger('app.createOrEditRiskModalSaved');

          if (typeof successCallback === 'function') {
            successCallback();
          }
        })
        .always(function () {
          abp.ui.clearBusy();
        });
    }

    function clearForm() {
      _$riskInformationForm[0].reset();
    }

    $('#saveBtn').click(function () {
      save(function () {
        window.location = '/App/Risks';
      });
    });

    $('#saveAndNewBtn').click(function () {
      save(function () {
        if (!$('input[name=id]').val()) {
          //if it is create page
          clearForm();
        }
      });
    });
  });
})();
