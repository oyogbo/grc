(function () {
  $(function () {
    var _$riskGroupingsTable = $('#RiskGroupingsTable');
    var _riskGroupingsService = abp.services.app.riskGroupings;

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
        getRiskGroupings();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getRiskGroupings();
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
        getRiskGroupings();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getRiskGroupings();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.RiskGroupings.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.RiskGroupings.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.RiskGroupings.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RiskGroupings/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RiskGroupings/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditRiskGroupingModal',
    });

    var _viewRiskGroupingModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RiskGroupings/ViewriskGroupingModal',
      modalClass: 'ViewRiskGroupingModal',
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

    var dataTable = _$riskGroupingsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _riskGroupingsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#RiskGroupingsTableFilter').val(),
            minValueFilter: $('#MinValueFilterId').val(),
            maxValueFilter: $('#MaxValueFilterId').val(),
            nameFilter: $('#NameFilterId').val(),
            minDefaultFilter: $('#MinDefaultFilterId').val(),
            maxDefaultFilter: $('#MaxDefaultFilterId').val(),
            minOrderFilter: $('#MinOrderFilterId').val(),
            maxOrderFilter: $('#MaxOrderFilterId').val(),
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
                  _viewRiskGroupingModal.open({ id: data.record.riskGrouping.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.riskGrouping.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteRiskGrouping(data.record.riskGrouping);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'riskGrouping.value',
          name: 'value',
        },
        {
          targets: 3,
          data: 'riskGrouping.name',
          name: 'name',
        },
        {
          targets: 4,
          data: 'riskGrouping.default',
          name: 'default',
        },
        {
          targets: 5,
          data: 'riskGrouping.order',
          name: 'order',
        },
      ],
    });

    function getRiskGroupings() {
      dataTable.ajax.reload();
    }

    function deleteRiskGrouping(riskGrouping) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _riskGroupingsService
            .delete({
              id: riskGrouping.id,
            })
            .done(function () {
              getRiskGroupings(true);
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

    $('#CreateNewRiskGroupingButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _riskGroupingsService
        .getRiskGroupingsToExcel({
          filter: $('#RiskGroupingsTableFilter').val(),
          minValueFilter: $('#MinValueFilterId').val(),
          maxValueFilter: $('#MaxValueFilterId').val(),
          nameFilter: $('#NameFilterId').val(),
          minDefaultFilter: $('#MinDefaultFilterId').val(),
          maxDefaultFilter: $('#MaxDefaultFilterId').val(),
          minOrderFilter: $('#MinOrderFilterId').val(),
          maxOrderFilter: $('#MaxOrderFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditRiskGroupingModalSaved', function () {
      getRiskGroupings();
    });

    $('#GetRiskGroupingsButton').click(function (e) {
      e.preventDefault();
      getRiskGroupings();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getRiskGroupings();
      }
    });

    $('.reload-on-change').change(function (e) {
      getRiskGroupings();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getRiskGroupings();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getRiskGroupings();
    });
  });
})();
