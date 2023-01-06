(function () {
  $(function () {
    var _$riskLevelsTable = $('#RiskLevelsTable');
    var _riskLevelsService = abp.services.app.riskLevels;

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
        getRiskLevels();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getRiskLevels();
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
        getRiskLevels();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getRiskLevels();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.RiskLevels.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.RiskLevels.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.RiskLevels.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RiskLevels/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RiskLevels/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditRiskLevelModal',
    });

    var _viewRiskLevelModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RiskLevels/ViewriskLevelModal',
      modalClass: 'ViewRiskLevelModal',
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

    var dataTable = _$riskLevelsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _riskLevelsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#RiskLevelsTableFilter').val(),
            minValueFilter: $('#MinValueFilterId').val(),
            maxValueFilter: $('#MaxValueFilterId').val(),
            nameFilter: $('#NameFilterId').val(),
            colorFilter: $('#ColorFilterId').val(),
            displayNameFilter: $('#DisplayNameFilterId').val(),
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
                  _viewRiskLevelModal.open({ id: data.record.riskLevel.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.riskLevel.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteRiskLevel(data.record.riskLevel);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'riskLevel.value',
          name: 'value',
        },
        {
          targets: 3,
          data: 'riskLevel.name',
          name: 'name',
        },
        {
          targets: 4,
          data: 'riskLevel.color',
          name: 'color',
        },
        {
          targets: 5,
          data: 'riskLevel.displayName',
          name: 'displayName',
        },
      ],
    });

    function getRiskLevels() {
      dataTable.ajax.reload();
    }

    function deleteRiskLevel(riskLevel) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _riskLevelsService
            .delete({
              id: riskLevel.id,
            })
            .done(function () {
              getRiskLevels(true);
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

    $('#CreateNewRiskLevelButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _riskLevelsService
        .getRiskLevelsToExcel({
          filter: $('#RiskLevelsTableFilter').val(),
          minValueFilter: $('#MinValueFilterId').val(),
          maxValueFilter: $('#MaxValueFilterId').val(),
          nameFilter: $('#NameFilterId').val(),
          colorFilter: $('#ColorFilterId').val(),
          displayNameFilter: $('#DisplayNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditRiskLevelModalSaved', function () {
      getRiskLevels();
    });

    $('#GetRiskLevelsButton').click(function (e) {
      e.preventDefault();
      getRiskLevels();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getRiskLevels();
      }
    });

    $('.reload-on-change').change(function (e) {
      getRiskLevels();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getRiskLevels();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getRiskLevels();
    });
  });
})();
