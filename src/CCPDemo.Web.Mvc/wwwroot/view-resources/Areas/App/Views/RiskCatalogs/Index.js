(function () {
  $(function () {
    var _$riskCatalogsTable = $('#RiskCatalogsTable');
    var _riskCatalogsService = abp.services.app.riskCatalogs;

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
        getRiskCatalogs();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getRiskCatalogs();
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
        getRiskCatalogs();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getRiskCatalogs();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.RiskCatalogs.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.RiskCatalogs.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.RiskCatalogs.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RiskCatalogs/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RiskCatalogs/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditRiskCatalogModal',
    });

    var _viewRiskCatalogModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RiskCatalogs/ViewriskCatalogModal',
      modalClass: 'ViewRiskCatalogModal',
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

    var dataTable = _$riskCatalogsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _riskCatalogsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#RiskCatalogsTableFilter').val(),
            numberFilter: $('#NumberFilterId').val(),
            minGroupingFilter: $('#MinGroupingFilterId').val(),
            maxGroupingFilter: $('#MaxGroupingFilterId').val(),
            nameFilter: $('#NameFilterId').val(),
            descriptionFilter: $('#DescriptionFilterId').val(),
            minFunctionFilter: $('#MinFunctionFilterId').val(),
            maxFunctionFilter: $('#MaxFunctionFilterId').val(),
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
                  _viewRiskCatalogModal.open({ id: data.record.riskCatalog.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.riskCatalog.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteRiskCatalog(data.record.riskCatalog);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'riskCatalog.number',
          name: 'number',
        },
        {
          targets: 3,
          data: 'riskCatalog.grouping',
          name: 'grouping',
        },
        {
          targets: 4,
          data: 'riskCatalog.name',
          name: 'name',
        },
        {
          targets: 5,
          data: 'riskCatalog.description',
          name: 'description',
        },
        {
          targets: 6,
          data: 'riskCatalog.function',
          name: 'function',
        },
        {
          targets: 7,
          data: 'riskCatalog.order',
          name: 'order',
        },
      ],
    });

    function getRiskCatalogs() {
      dataTable.ajax.reload();
    }

    function deleteRiskCatalog(riskCatalog) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _riskCatalogsService
            .delete({
              id: riskCatalog.id,
            })
            .done(function () {
              getRiskCatalogs(true);
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

    $('#CreateNewRiskCatalogButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _riskCatalogsService
        .getRiskCatalogsToExcel({
          filter: $('#RiskCatalogsTableFilter').val(),
          numberFilter: $('#NumberFilterId').val(),
          minGroupingFilter: $('#MinGroupingFilterId').val(),
          maxGroupingFilter: $('#MaxGroupingFilterId').val(),
          nameFilter: $('#NameFilterId').val(),
          descriptionFilter: $('#DescriptionFilterId').val(),
          minFunctionFilter: $('#MinFunctionFilterId').val(),
          maxFunctionFilter: $('#MaxFunctionFilterId').val(),
          minOrderFilter: $('#MinOrderFilterId').val(),
          maxOrderFilter: $('#MaxOrderFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditRiskCatalogModalSaved', function () {
      getRiskCatalogs();
    });

    $('#GetRiskCatalogsButton').click(function (e) {
      e.preventDefault();
      getRiskCatalogs();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getRiskCatalogs();
      }
    });

    $('.reload-on-change').change(function (e) {
      getRiskCatalogs();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getRiskCatalogs();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getRiskCatalogs();
    });
  });
})();
