(function () {
    var $ = jQuery
  $(function () {
    var _$riskStatusTable = $('#RiskStatusTable');
    var _riskStatusService = abp.services.app.riskStatus;

    //$('.date-picker').datetimepicker({
    //  locale: abp.localization.currentLanguage.name,
    //  format: 'L',
    //});

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.RiskStatus.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.RiskStatus.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.RiskStatus.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RiskStatus/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RiskStatus/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditRiskStatusModal',
    });

    var _viewRiskStatusModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RiskStatus/ViewriskStatusModal',
      modalClass: 'ViewRiskStatusModal',
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

    var dataTable = _$riskStatusTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _riskStatusService.getAll,
        inputFilter: function () {
          return {
            filter: $('#RiskStatusTableFilter').val(),
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
                  _viewRiskStatusModal.open({ id: data.record.riskStatus.id });
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.riskStatus.id });
                },
              },
              {
                text: app.localize('Delete'),
                iconStyle: 'far fa-trash-alt mr-2',
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteRiskStatus(data.record.riskStatus);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'riskStatus.name',
          name: 'name',
        },
      ],
    });

    function getRiskStatus() {
      dataTable.ajax.reload();
    }

    function deleteRiskStatus(riskStatus) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _riskStatusService
            .delete({
              id: riskStatus.id,
            })
            .done(function () {
              getRiskStatus(true);
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

    $('#CreateNewRiskStatusButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _riskStatusService
        .getRiskStatusToExcel({
          filter: $('#RiskStatusTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditRiskStatusModalSaved', function () {
      getRiskStatus();
    });

    $('#GetRiskStatusButton').click(function (e) {
      e.preventDefault();
      getRiskStatus();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getRiskStatus();
      }
    });
  });
})();
