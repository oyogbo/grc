(function () {
  $(function () {
    var _$riskFunctionsTable = $('#RiskFunctionsTable');
    var _riskFunctionsService = abp.services.app.riskFunctions;

    var $selectedDate = {
      startDate: null,
      endDate: null,
    };

    $('.date-picker').on('apply.daterangepicker', function (ev, picker) {
      $(this).val(picker.startDate.format('MM/DD/YYYY'));
    });

    $('.startDate')
      .daterangepicker({
        autoUpdateInput: false,
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      })
      .on('apply.daterangepicker', (ev, picker) => {
        $selectedDate.startDate = picker.startDate;
        getRiskFunctions();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getRiskFunctions();
      });

    $('.endDate')
      .daterangepicker({
        autoUpdateInput: false,
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      })
      .on('apply.daterangepicker', (ev, picker) => {
        $selectedDate.endDate = picker.startDate;
        getRiskFunctions();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getRiskFunctions();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.RiskFunctions.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.RiskFunctions.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.RiskFunctions.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RiskFunctions/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RiskFunctions/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditRiskFunctionModal',
    });

    var _viewRiskFunctionModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RiskFunctions/ViewriskFunctionModal',
      modalClass: 'ViewRiskFunctionModal',
    });

    var getDateFilter = function (element) {
      if ($selectedDate.startDate == null) {
        return null;
      }
      return $selectedDate.startDate.format('YYYY-MM-DDT00:00:00Z');
    };

    var getMaxDateFilter = function (element) {
      if ($selectedDate.endDate == null) {
        return null;
      }
      return $selectedDate.endDate.format('YYYY-MM-DDT23:59:59Z');
    };

    var dataTable = _$riskFunctionsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _riskFunctionsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#RiskFunctionsTableFilter').val(),
            minValueFilter: $('#MinValueFilterId').val(),
            maxValueFilter: $('#MaxValueFilterId').val(),
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
                action: function (data) {
                  _viewRiskFunctionModal.open({ id: data.record.riskFunction.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.riskFunction.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteRiskFunction(data.record.riskFunction);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'riskFunction.value',
          name: 'value',
        },
        {
          targets: 3,
          data: 'riskFunction.name',
          name: 'name',
        },
      ],
    });

    function getRiskFunctions() {
      dataTable.ajax.reload();
    }

    function deleteRiskFunction(riskFunction) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _riskFunctionsService
            .delete({
              id: riskFunction.id,
            })
            .done(function () {
              getRiskFunctions(true);
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

    $('#CreateNewRiskFunctionButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _riskFunctionsService
        .getRiskFunctionsToExcel({
          filter: $('#RiskFunctionsTableFilter').val(),
          minValueFilter: $('#MinValueFilterId').val(),
          maxValueFilter: $('#MaxValueFilterId').val(),
          nameFilter: $('#NameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditRiskFunctionModalSaved', function () {
      getRiskFunctions();
    });

    $('#GetRiskFunctionsButton').click(function (e) {
      e.preventDefault();
      getRiskFunctions();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getRiskFunctions();
      }
    });

    $('.reload-on-change').change(function (e) {
      getRiskFunctions();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getRiskFunctions();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getRiskFunctions();
    });
  });
})();
