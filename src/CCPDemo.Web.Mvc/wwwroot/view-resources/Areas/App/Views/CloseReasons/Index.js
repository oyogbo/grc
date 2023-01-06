(function () {
  $(function () {
    var _$closeReasonsTable = $('#CloseReasonsTable');
    var _closeReasonsService = abp.services.app.closeReasons;

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
        getCloseReasons();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getCloseReasons();
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
        getCloseReasons();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getCloseReasons();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.CloseReasons.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.CloseReasons.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.CloseReasons.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/CloseReasons/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CloseReasons/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditCloseReasonModal',
    });

    var _viewCloseReasonModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/CloseReasons/ViewcloseReasonModal',
      modalClass: 'ViewCloseReasonModal',
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

    var dataTable = _$closeReasonsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _closeReasonsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#CloseReasonsTableFilter').val(),
            minvalueFilter: $('#MinvalueFilterId').val(),
            maxvalueFilter: $('#MaxvalueFilterId').val(),
            nameFilter: $('#nameFilterId').val(),
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
                  _viewCloseReasonModal.open({ id: data.record.closeReason.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.closeReason.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteCloseReason(data.record.closeReason);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'closeReason.value',
          name: 'value',
        },
        {
          targets: 3,
          data: 'closeReason.name',
          name: 'name',
        },
      ],
    });

    function getCloseReasons() {
      dataTable.ajax.reload();
    }

    function deleteCloseReason(closeReason) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _closeReasonsService
            .delete({
              id: closeReason.id,
            })
            .done(function () {
              getCloseReasons(true);
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

    $('#CreateNewCloseReasonButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _closeReasonsService
        .getCloseReasonsToExcel({
          filter: $('#CloseReasonsTableFilter').val(),
          minvalueFilter: $('#MinvalueFilterId').val(),
          maxvalueFilter: $('#MaxvalueFilterId').val(),
          nameFilter: $('#nameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditCloseReasonModalSaved', function () {
      getCloseReasons();
    });

    $('#GetCloseReasonsButton').click(function (e) {
      e.preventDefault();
      getCloseReasons();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getCloseReasons();
      }
    });

    $('.reload-on-change').change(function (e) {
      getCloseReasons();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getCloseReasons();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getCloseReasons();
    });
  });
})();
