(function () {
  $(function () {
    var _$closuresTable = $('#ClosuresTable');
    var _closuresService = abp.services.app.closures;

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
        getClosures();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getClosures();
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
        getClosures();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getClosures();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.Closures.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.Closures.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.Closures.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Closures/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Closures/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditClosureModal',
    });

    var _viewClosureModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Closures/ViewclosureModal',
      modalClass: 'ViewClosureModal',
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

    var dataTable = _$closuresTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _closuresService.getAll,
        inputFilter: function () {
          return {
            filter: $('#ClosuresTableFilter').val(),
            minRiskIdFilter: $('#MinRiskIdFilterId').val(),
            maxRiskIdFilter: $('#MaxRiskIdFilterId').val(),
            minUserIdFilter: $('#MinUserIdFilterId').val(),
            maxUserIdFilter: $('#MaxUserIdFilterId').val(),
            minClosureDateFilter: getDateFilter($('#MinClosureDateFilterId')),
            maxClosureDateFilter: getMaxDateFilter($('#MaxClosureDateFilterId')),
            minCloseReasonIdFilter: $('#MinCloseReasonIdFilterId').val(),
            maxCloseReasonIdFilter: $('#MaxCloseReasonIdFilterId').val(),
            noteFilter: $('#NoteFilterId').val(),
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
                  _viewClosureModal.open({ id: data.record.closure.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.closure.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteClosure(data.record.closure);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'closure.riskId',
          name: 'riskId',
        },
        {
          targets: 3,
          data: 'closure.userId',
          name: 'userId',
        },
        {
          targets: 4,
          data: 'closure.closureDate',
          name: 'closureDate',
          render: function (closureDate) {
            if (closureDate) {
              return moment(closureDate).format('L');
            }
            return '';
          },
        },
        {
          targets: 5,
          data: 'closure.closeReasonId',
          name: 'closeReasonId',
        },
        {
          targets: 6,
          data: 'closure.note',
          name: 'note',
        },
      ],
    });

    function getClosures() {
      dataTable.ajax.reload();
    }

    function deleteClosure(closure) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _closuresService
            .delete({
              id: closure.id,
            })
            .done(function () {
              getClosures(true);
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

    $('#CreateNewClosureButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _closuresService
        .getClosuresToExcel({
          filter: $('#ClosuresTableFilter').val(),
          minRiskIdFilter: $('#MinRiskIdFilterId').val(),
          maxRiskIdFilter: $('#MaxRiskIdFilterId').val(),
          minUserIdFilter: $('#MinUserIdFilterId').val(),
          maxUserIdFilter: $('#MaxUserIdFilterId').val(),
          minClosureDateFilter: getDateFilter($('#MinClosureDateFilterId')),
          maxClosureDateFilter: getMaxDateFilter($('#MaxClosureDateFilterId')),
          minCloseReasonIdFilter: $('#MinCloseReasonIdFilterId').val(),
          maxCloseReasonIdFilter: $('#MaxCloseReasonIdFilterId').val(),
          noteFilter: $('#NoteFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditClosureModalSaved', function () {
      getClosures();
    });

    $('#GetClosuresButton').click(function (e) {
      e.preventDefault();
      getClosures();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getClosures();
      }
    });

    $('.reload-on-change').change(function (e) {
      getClosures();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getClosures();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getClosures();
    });
  });
})();
