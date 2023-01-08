(function () {
  $(function () {
    var _usersLookupsService = abp.services.app.usersLookups;

    var _$usersLookupInformationForm = $('form[name=UsersLookupInformationsForm]');
    _$usersLookupInformationForm.validate();

    var _UsersLookupuserLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/UsersLookups/UserLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/UsersLookups/_UsersLookupUserLookupTableModal.js',
      modalClass: 'UserLookupTableModal',
    });

    $('#OpenUserLookupTableButton').click(function () {
      var usersLookup = _$usersLookupInformationForm.serializeFormToObject();

      _UsersLookupuserLookupTableModal.open(
        { id: usersLookup.userId, displayName: usersLookup.userName },
        function (data) {
          _$usersLookupInformationForm.find('input[name=userName]').val(data.displayName);
          _$usersLookupInformationForm.find('input[name=userId]').val(data.id);
        }
      );
    });

    $('#ClearUserNameButton').click(function () {
      _$usersLookupInformationForm.find('input[name=userName]').val('');
      _$usersLookupInformationForm.find('input[name=userId]').val('');
    });

    function save(successCallback) {
      if (!_$usersLookupInformationForm.valid()) {
        return;
      }
      if ($('#UsersLookup_UserId').prop('required') && $('#UsersLookup_UserId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
        return;
      }

      var usersLookup = _$usersLookupInformationForm.serializeFormToObject();

      abp.ui.setBusy();
      _usersLookupsService
        .createOrEdit(usersLookup)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          abp.event.trigger('app.createOrEditUsersLookupModalSaved');

          if (typeof successCallback === 'function') {
            successCallback();
          }
        })
        .always(function () {
          abp.ui.clearBusy();
        });
    }

    function clearForm() {
      _$usersLookupInformationForm[0].reset();
    }

    $('#saveBtn').click(function () {
      save(function () {
        window.location = '/App/UsersLookups';
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
