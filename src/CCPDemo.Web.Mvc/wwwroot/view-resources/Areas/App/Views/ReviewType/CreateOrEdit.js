(function () {
  $(function () {
    var _reviewTypeService = abp.services.app.reviewType;

    var _$reviewTypeInformationForm = $('form[name=ReviewTypeInformationsForm]');
    _$reviewTypeInformationForm.validate();

    $('.date-picker').datetimepicker({
      locale: abp.localization.currentLanguage.name,
      format: 'L',
    });

    function save(successCallback) {
      if (!_$reviewTypeInformationForm.valid()) {
        return;
      }

      var reviewType = _$reviewTypeInformationForm.serializeFormToObject();

      abp.ui.setBusy();
      _reviewTypeService
        .createOrEdit(reviewType)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          abp.event.trigger('app.createOrEditReviewTypeModalSaved');

          if (typeof successCallback === 'function') {
            successCallback();
          }
        })
        .always(function () {
          abp.ui.clearBusy();
        });
    }

    function clearForm() {
      _$reviewTypeInformationForm[0].reset();
    }

    $('#saveBtn').click(function () {
      save(function () {
        window.location = '/App/ReviewType';
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
