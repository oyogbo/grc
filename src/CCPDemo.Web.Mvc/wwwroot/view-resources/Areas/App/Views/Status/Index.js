(function () {
    var $ = jQuery
  $(function () {
    var _$statusTable = $('#StatusTable');
    var _statusService = abp.services.app.status;

    //$('.date-picker').datetimepicker({
    //  locale: abp.localization.currentLanguage.name,
    //  format: 'L',
    //});

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.Status.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.Status.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.Status.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Status/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Status/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditStatusModal',
    });

    var _viewStatusModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Status/ViewstatusModal',
      modalClass: 'ViewStatusModal',
    });

    var getDateFilter = function (element) {
      if (element.data('DateTimePicker').date() == null) {
        return null;
      }
      return element.data('DateTimePicker').date().format('YYYY-MM-DDT00:00:00Z');
    };

    var getMaxDateFilter = function (element) {
      if (element.data('DateTimePicker').date() == null) {
        return null;
      }
      return element.data('DateTimePicker').date().format('YYYY-MM-DDT23:59:59Z');
    };

    var dataTable = _$statusTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _statusService.getAll,
        inputFilter: function () {
          return {
            filter: $('#StatusTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
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
                  _viewStatusModal.open({ id: data.record.status.id });
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.status.id });
                },
              },
              {
                text: app.localize('Delete'),
                iconStyle: 'far fa-trash-alt mr-2',
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteStatus(data.record.status);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'status.name',
          name: 'name',
        },
      ],
    });

    function getStatus() {
      dataTable.ajax.reload();
    }

    function deleteStatus(status) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _statusService
            .delete({
              id: status.id,
            })
            .done(function () {
              getStatus(true);
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

    $('#CreateNewStatusButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _statusService
        .getStatusToExcel({
          filter: $('#StatusTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditStatusModalSaved', function () {
      getStatus();
    });

    $('#GetStatusButton').click(function (e) {
      e.preventDefault();
      getStatus();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getStatus();
      }
    });
  });
})();
