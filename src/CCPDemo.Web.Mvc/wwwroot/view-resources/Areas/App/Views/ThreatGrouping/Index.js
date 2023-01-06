(function () {
  $(function () {
    var _$threatGroupingTable = $('#ThreatGroupingTable');
    var _threatGroupingService = abp.services.app.threatGrouping;

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
        getThreatGrouping();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getThreatGrouping();
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
        getThreatGrouping();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getThreatGrouping();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.ThreatGrouping.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.ThreatGrouping.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.ThreatGrouping.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/ThreatGrouping/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ThreatGrouping/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditThreatGroupingModal',
    });

    var _viewThreatGroupingModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/ThreatGrouping/ViewthreatGroupingModal',
      modalClass: 'ViewThreatGroupingModal',
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

    var dataTable = _$threatGroupingTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _threatGroupingService.getAll,
        inputFilter: function () {
          return {
            filter: $('#ThreatGroupingTableFilter').val(),
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
                  _viewThreatGroupingModal.open({ id: data.record.threatGrouping.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.threatGrouping.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteThreatGrouping(data.record.threatGrouping);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'threatGrouping.value',
          name: 'value',
        },
        {
          targets: 3,
          data: 'threatGrouping.name',
          name: 'name',
        },
        {
          targets: 4,
          data: 'threatGrouping.default',
          name: 'default',
        },
        {
          targets: 5,
          data: 'threatGrouping.order',
          name: 'order',
        },
      ],
    });

    function getThreatGrouping() {
      dataTable.ajax.reload();
    }

    function deleteThreatGrouping(threatGrouping) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _threatGroupingService
            .delete({
              id: threatGrouping.id,
            })
            .done(function () {
              getThreatGrouping(true);
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

    $('#CreateNewThreatGroupingButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _threatGroupingService
        .getThreatGroupingToExcel({
          filter: $('#ThreatGroupingTableFilter').val(),
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

    abp.event.on('app.createOrEditThreatGroupingModalSaved', function () {
      getThreatGrouping();
    });

    $('#GetThreatGroupingButton').click(function (e) {
      e.preventDefault();
      getThreatGrouping();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getThreatGrouping();
      }
    });

    $('.reload-on-change').change(function (e) {
      getThreatGrouping();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getThreatGrouping();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getThreatGrouping();
    });
  });
})();
