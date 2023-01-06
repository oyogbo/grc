(function () {
  $(function () {
    var _mitigationsService = abp.services.app.mitigations;

    var _$mitigationInformationForm = $('form[name=MitigationInformationsForm]');
    _$mitigationInformationForm.validate();

    $('.date-picker').datetimepicker({
      locale: abp.localization.currentLanguage.name,
      format: 'L',
    });

    function save(successCallback) {
      if (!_$mitigationInformationForm.valid()) {
        return;
      }

      var mitigation = _$mitigationInformationForm.serializeFormToObject();

      abp.ui.setBusy();
      _mitigationsService
        .createOrEdit(mitigation)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          abp.event.trigger('app.createOrEditMitigationModalSaved');

          if (typeof successCallback === 'function') {
            successCallback();
          }
        })
        .always(function () {
          abp.ui.clearBusy();
        });
    }

    function clearForm() {
      _$mitigationInformationForm[0].reset();
    }

    $('#saveBtn').click(function () {
      save(function () {
        window.location = '/App/Mitigations';
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
