(function ($) {
  app.modals.UserLookupTableModal = function () {
    var _modalManager;

    var _usersLookupsService = abp.services.app.usersLookups;
    var _$userTable = $('#UserTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$userTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _usersLookupsService.getAllUserForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#UserTableFilter').val(),
          };
        },
      },
      columnDefs: [
        {
          targets: 0,
          data: null,
          orderable: false,
          autoWidth: false,
          defaultContent:
            "<div class=\"text-center\"><input id='selectbtn' class='btn btn-success' type='button' width='25px' value='" +
            app.localize('Select') +
            "' /></div>",
        },
        {
          autoWidth: false,
          orderable: false,
          targets: 1,
          data: 'displayName',
        },
      ],
    });

    $('#UserTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getUser() {
      dataTable.ajax.reload();
    }

    $('#GetUserButton').click(function (e) {
      e.preventDefault();
      getUser();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getUser();
      }
    });
  };
})(jQuery);
