(function () {
  $(function () {
    var _$riskModelsTable = $('#RiskModelsTable');
    var _riskModelsService = abp.services.app.riskModels;

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
        getRiskModels();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getRiskModels();
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
        getRiskModels();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getRiskModels();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.RiskModels.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.RiskModels.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.RiskModels.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RiskModels/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RiskModels/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditRiskModelModal',
    });

    var _viewRiskModelModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RiskModels/ViewriskModelModal',
      modalClass: 'ViewRiskModelModal',
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

    var dataTable = _$riskModelsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _riskModelsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#RiskModelsTableFilter').val(),
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
                  _viewRiskModelModal.open({ id: data.record.riskModel.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.riskModel.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteRiskModel(data.record.riskModel);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'riskModel.value',
          name: 'value',
        },
        {
          targets: 3,
          data: 'riskModel.name',
          name: 'name',
        },
      ],
    });

    function getRiskModels() {
      dataTable.ajax.reload();
    }

    function deleteRiskModel(riskModel) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _riskModelsService
            .delete({
              id: riskModel.id,
            })
            .done(function () {
              getRiskModels(true);
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

    $('#CreateNewRiskModelButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _riskModelsService
        .getRiskModelsToExcel({
          filter: $('#RiskModelsTableFilter').val(),
          minValueFilter: $('#MinValueFilterId').val(),
          maxValueFilter: $('#MaxValueFilterId').val(),
          nameFilter: $('#NameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditRiskModelModalSaved', function () {
      getRiskModels();
    });

    $('#GetRiskModelsButton').click(function (e) {
      e.preventDefault();
      getRiskModels();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getRiskModels();
      }
    });

    $('.reload-on-change').change(function (e) {
      getRiskModels();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getRiskModels();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getRiskModels();
    });
  });
})();
