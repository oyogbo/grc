(function () {
  $(function () {
    var _$regulationsTable = $('#RegulationsTable');
    var _regulationsService = abp.services.app.regulations;

    $('.date-picker').datetimepicker({
      locale: abp.localization.currentLanguage.name,
      format: 'L',
    });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.Regulations.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.Regulations.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.Regulations.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Regulations/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Regulations/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditRegulationModal',
    });

    var _viewRegulationModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Regulations/ViewregulationModal',
      modalClass: 'ViewRegulationModal',
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

    var dataTable = _$regulationsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _regulationsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#RegulationsTableFilter').val(),
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
                iconStyle: 'far fa-eye mr-2',
                action: function (data) {
                  _viewRegulationModal.open({ id: data.record.regulation.id });
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.regulation.id });
                },
              },
              {
                text: app.localize('Delete'),
                iconStyle: 'far fa-trash-alt mr-2',
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteRegulation(data.record.regulation);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'regulation.value',
          name: 'value',
        },
        {
          targets: 3,
          data: 'regulation.name',
          name: 'name',
        },
      ],
    });

    function getRegulations() {
      dataTable.ajax.reload();
    }

    function deleteRegulation(regulation) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _regulationsService
            .delete({
              id: regulation.id,
            })
            .done(function () {
              getRegulations(true);
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

    $('#CreateNewRegulationButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _regulationsService
        .getRegulationsToExcel({
          filter: $('#RegulationsTableFilter').val(),
          minvalueFilter: $('#MinvalueFilterId').val(),
          maxvalueFilter: $('#MaxvalueFilterId').val(),
          nameFilter: $('#nameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditRegulationModalSaved', function () {
      getRegulations();
    });

    $('#GetRegulationsButton').click(function (e) {
      e.preventDefault();
      getRegulations();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getRegulations();
      }
    });
  });
})();
