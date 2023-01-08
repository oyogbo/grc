(function () {
  $(function () {
    var _$usersLookupsTable = $('#UsersLookupsTable');
    var _usersLookupsService = abp.services.app.usersLookups;

    var _permissions = {
      create: abp.auth.hasPermission('Pages.UsersLookups.Create'),
      edit: abp.auth.hasPermission('Pages.UsersLookups.Edit'),
      delete: abp.auth.hasPermission('Pages.UsersLookups.Delete'),
    };

    var _viewUsersLookupModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/UsersLookups/ViewusersLookupModal',
      modalClass: 'ViewUsersLookupModal',
    });

    //var getDateFilter = function (element) {
    //  if (element.data('DateTimePicker').date() == null) {
    //    return null;
    //  }
    //  return element.data('DateTimePicker').date().format('YYYY-MM-DDT00:00:00Z');
    //};

    //var getMaxDateFilter = function (element) {
    //  if (element.data('DateTimePicker').date() == null) {
    //    return null;
    //  }
    //  return element.data('DateTimePicker').date().format('YYYY-MM-DDT23:59:59Z');
    //};

    var dataTable = _$usersLookupsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _usersLookupsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#UsersLookupsTableFilter').val(),
            minUserFilter: $('#MinUserFilterId').val(),
            maxUserFilter: $('#MaxUserFilterId').val(),
            userNameFilter: $('#UserNameFilterId').val(),
          };
        },
      },
      columnDefs: [
        {
          className: 'control responsive',
          orderable: false,
          render: function () {
            return '';
          },
          targets: 0,
        },
        {
          width: 120,
          targets: 1,
          data: null,
          orderable: false,
          autoWidth: false,
          defaultContent: '',
          rowAction: {
            cssClass: 'btn btn-brand dropdown-toggle',
            text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
            items: [
              {
                text: app.localize('View'),
                iconStyle: 'far fa-eye mr-2',
                action: function (data) {
                  window.location = '/App/UsersLookups/ViewUsersLookup/' + data.record.usersLookup.id;
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  window.location = '/App/UsersLookups/CreateOrEdit/' + data.record.usersLookup.id;
                },
              },
              {
                text: app.localize('Delete'),
                iconStyle: 'far fa-trash-alt mr-2',
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteUsersLookup(data.record.usersLookup);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'usersLookup.user',
          name: 'user',
        },
        {
          targets: 3,
          data: 'userName',
          name: 'userFk.name',
        },
      ],
    });

    function getUsersLookups() {
      dataTable.ajax.reload();
    }

    function deleteUsersLookup(usersLookup) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _usersLookupsService
            .delete({
              id: usersLookup.id,
            })
            .done(function () {
              getUsersLookups(true);
              abp.notify.success(app.localize('SuccessfullyDeleted'));
            });
        }
      });
    }

    $('#ShowAdvancedFiltersSpan').click(function () {
      $('#ShowAdvancedFiltersSpan').hide();
      $('#HideAdvancedFiltersSpan').show();
      $('#AdvacedAuditFiltersArea').slideDown();
    });

    $('#HideAdvancedFiltersSpan').click(function () {
      $('#HideAdvancedFiltersSpan').hide();
      $('#ShowAdvancedFiltersSpan').show();
      $('#AdvacedAuditFiltersArea').slideUp();
    });

    $('#ExportToExcelButton').click(function () {
      _usersLookupsService
        .getUsersLookupsToExcel({
          filter: $('#UsersLookupsTableFilter').val(),
          minUserFilter: $('#MinUserFilterId').val(),
          maxUserFilter: $('#MaxUserFilterId').val(),
          userNameFilter: $('#UserNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditUsersLookupModalSaved', function () {
      getUsersLookups();
    });

    $('#GetUsersLookupsButton').click(function (e) {
      e.preventDefault();
      getUsersLookups();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getUsersLookups();
      }
    });
  });
})();
