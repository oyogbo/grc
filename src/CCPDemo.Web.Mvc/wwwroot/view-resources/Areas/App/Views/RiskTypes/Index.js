(function () {
  $(function () {
    var _$riskTypesTable = $('#RiskTypesTable');
    var _riskTypesService = abp.services.app.riskTypes;

    //$('.date-picker').datetimepicker({
    //  locale: abp.localization.currentLanguage.name,
    //  format: 'L',
    //});

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.RiskTypes.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.RiskTypes.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.RiskTypes.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RiskTypes/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RiskTypes/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditRiskTypeModal',
    });

    var _viewRiskTypeModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RiskTypes/ViewriskTypeModal',
      modalClass: 'ViewRiskTypeModal',
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

    var dataTable = _$riskTypesTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _riskTypesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#RiskTypesTableFilter').val(),
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
                  _viewRiskTypeModal.open({ id: data.record.riskType.id });
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.riskType.id });
                },
              },
              {
                text: app.localize('Delete'),
                iconStyle: 'far fa-trash-alt mr-2',
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteRiskType(data.record.riskType);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'riskType.name',
          name: 'name',
        },
      ],
    });

    function getRiskTypes() {
      dataTable.ajax.reload();
    }

    function deleteRiskType(riskType) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _riskTypesService
            .delete({
              id: riskType.id,
            })
            .done(function () {
              getRiskTypes(true);
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

    $('#CreateNewRiskTypeButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _riskTypesService
        .getRiskTypesToExcel({
          filter: $('#RiskTypesTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditRiskTypeModalSaved', function () {
      getRiskTypes();
    });

    $('#GetRiskTypesButton').click(function (e) {
      e.preventDefault();
      getRiskTypes();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getRiskTypes();
      }
    });
  });
})();
