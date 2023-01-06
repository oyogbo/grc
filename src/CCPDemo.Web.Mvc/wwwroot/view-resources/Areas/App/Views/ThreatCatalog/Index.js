(function () {
  $(function () {
    var _$threatCatalogTable = $('#ThreatCatalogTable');
    var _threatCatalogService = abp.services.app.threatCatalog;

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
        getThreatCatalog();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getThreatCatalog();
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
        getThreatCatalog();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getThreatCatalog();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.ThreatCatalog.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.ThreatCatalog.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.ThreatCatalog.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/ThreatCatalog/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ThreatCatalog/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditThreatCatalogModal',
    });

    var _viewThreatCatalogModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/ThreatCatalog/ViewthreatCatalogModal',
      modalClass: 'ViewThreatCatalogModal',
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

    var dataTable = _$threatCatalogTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _threatCatalogService.getAll,
        inputFilter: function () {
          return {
            filter: $('#ThreatCatalogTableFilter').val(),
            numberFilter: $('#NumberFilterId').val(),
            minGroupingFilter: $('#MinGroupingFilterId').val(),
            maxGroupingFilter: $('#MaxGroupingFilterId').val(),
            nameFilter: $('#NameFilterId').val(),
            descriptionFilter: $('#DescriptionFilterId').val(),
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
                  _viewThreatCatalogModal.open({ id: data.record.threatCatalog.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.threatCatalog.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteThreatCatalog(data.record.threatCatalog);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'threatCatalog.number',
          name: 'number',
        },
        {
          targets: 3,
          data: 'threatCatalog.grouping',
          name: 'grouping',
        },
        {
          targets: 4,
          data: 'threatCatalog.name',
          name: 'name',
        },
        {
          targets: 5,
          data: 'threatCatalog.description',
          name: 'description',
        },
        {
          targets: 6,
          data: 'threatCatalog.order',
          name: 'order',
        },
      ],
    });

    function getThreatCatalog() {
      dataTable.ajax.reload();
    }

    function deleteThreatCatalog(threatCatalog) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _threatCatalogService
            .delete({
              id: threatCatalog.id,
            })
            .done(function () {
              getThreatCatalog(true);
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

    $('#CreateNewThreatCatalogButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _threatCatalogService
        .getThreatCatalogToExcel({
          filter: $('#ThreatCatalogTableFilter').val(),
          numberFilter: $('#NumberFilterId').val(),
          minGroupingFilter: $('#MinGroupingFilterId').val(),
          maxGroupingFilter: $('#MaxGroupingFilterId').val(),
          nameFilter: $('#NameFilterId').val(),
          descriptionFilter: $('#DescriptionFilterId').val(),
          minOrderFilter: $('#MinOrderFilterId').val(),
          maxOrderFilter: $('#MaxOrderFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditThreatCatalogModalSaved', function () {
      getThreatCatalog();
    });

    $('#GetThreatCatalogButton').click(function (e) {
      e.preventDefault();
      getThreatCatalog();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getThreatCatalog();
      }
    });

    $('.reload-on-change').change(function (e) {
      getThreatCatalog();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getThreatCatalog();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getThreatCatalog();
    });
  });
})();
