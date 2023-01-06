(function () {
  $(function () {
    var _$mitigationsTable = $('#MitigationsTable');
    var _mitigationsService = abp.services.app.mitigations;

    $('.date-picker').datetimepicker({
      locale: abp.localization.currentLanguage.name,
      format: 'L',
    });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Mitigations.Create'),
      edit: abp.auth.hasPermission('Pages.Mitigations.Edit'),
      delete: abp.auth.hasPermission('Pages.Mitigations.Delete'),
    };

    var _viewMitigationModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Mitigations/ViewmitigationModal',
      modalClass: 'ViewMitigationModal',
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

    var dataTable = _$mitigationsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _mitigationsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#MitigationsTableFilter').val(),
            minrisk_idFilter: $('#Minrisk_idFilterId').val(),
            maxrisk_idFilter: $('#Maxrisk_idFilterId').val(),
            minsubmission_dateFilter: getDateFilter($('#Minsubmission_dateFilterId')),
            maxsubmission_dateFilter: getMaxDateFilter($('#Maxsubmission_dateFilterId')),
            minlast_updateFilter: getDateFilter($('#Minlast_updateFilterId')),
            maxlast_updateFilter: getMaxDateFilter($('#Maxlast_updateFilterId')),
            minplanning_strategyFilter: $('#Minplanning_strategyFilterId').val(),
            maxplanning_strategyFilter: $('#Maxplanning_strategyFilterId').val(),
            minmitigation_effortFilter: $('#Minmitigation_effortFilterId').val(),
            maxmitigation_effortFilter: $('#Maxmitigation_effortFilterId').val(),
            minmitigation_costFilter: $('#Minmitigation_costFilterId').val(),
            maxmitigation_costFilter: $('#Maxmitigation_costFilterId').val(),
            minmitigation_ownerFilter: $('#Minmitigation_ownerFilterId').val(),
            maxmitigation_ownerFilter: $('#Maxmitigation_ownerFilterId').val(),
            current_solutionFilter: $('#current_solutionFilterId').val(),
            security_requirementsFilter: $('#security_requirementsFilterId').val(),
            security_recommendationsFilter: $('#security_recommendationsFilterId').val(),
            minsubmitted_byFilter: $('#Minsubmitted_byFilterId').val(),
            maxsubmitted_byFilter: $('#Maxsubmitted_byFilterId').val(),
            minplanning_dateFilter: getDateFilter($('#Minplanning_dateFilterId')),
            maxplanning_dateFilter: getMaxDateFilter($('#Maxplanning_dateFilterId')),
            minmitigation_percentFilter: $('#Minmitigation_percentFilterId').val(),
            maxmitigation_percentFilter: $('#Maxmitigation_percentFilterId').val(),
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
                  window.location = '/App/Mitigations/ViewMitigation/' + data.record.mitigation.id;
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  window.location = '/App/Mitigations/CreateOrEdit/' + data.record.mitigation.id;
                },
              },
              {
                text: app.localize('Delete'),
                iconStyle: 'far fa-trash-alt mr-2',
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteMitigation(data.record.mitigation);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'mitigation.risk_id',
          name: 'risk_id',
        },
        {
          targets: 3,
          data: 'mitigation.submission_date',
          name: 'submission_date',
          render: function (submission_date) {
            if (submission_date) {
              return moment(submission_date).format('L');
            }
            return '';
          },
        },
        {
          targets: 4,
          data: 'mitigation.last_update',
          name: 'last_update',
          render: function (last_update) {
            if (last_update) {
              return moment(last_update).format('L');
            }
            return '';
          },
        },
        {
          targets: 5,
          data: 'mitigation.planning_strategy',
          name: 'planning_strategy',
        },
        {
          targets: 6,
          data: 'mitigation.mitigation_effort',
          name: 'mitigation_effort',
        },
        {
          targets: 7,
          data: 'mitigation.mitigation_cost',
          name: 'mitigation_cost',
        },
        {
          targets: 8,
          data: 'mitigation.mitigation_owner',
          name: 'mitigation_owner',
        },
        {
          targets: 9,
          data: 'mitigation.current_solution',
          name: 'current_solution',
        },
        {
          targets: 10,
          data: 'mitigation.security_requirements',
          name: 'security_requirements',
        },
        {
          targets: 11,
          data: 'mitigation.security_recommendations',
          name: 'security_recommendations',
        },
        {
          targets: 12,
          data: 'mitigation.submitted_by',
          name: 'submitted_by',
        },
        {
          targets: 13,
          data: 'mitigation.planning_date',
          name: 'planning_date',
          render: function (planning_date) {
            if (planning_date) {
              return moment(planning_date).format('L');
            }
            return '';
          },
        },
        {
          targets: 14,
          data: 'mitigation.mitigation_percent',
          name: 'mitigation_percent',
        },
      ],
    });

    function getMitigations() {
      dataTable.ajax.reload();
    }

    function deleteMitigation(mitigation) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _mitigationsService
            .delete({
              id: mitigation.id,
            })
            .done(function () {
              getMitigations(true);
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

    $('#ExportToExcelButton').click(function () {
      _mitigationsService
        .getMitigationsToExcel({
          filter: $('#MitigationsTableFilter').val(),
          minrisk_idFilter: $('#Minrisk_idFilterId').val(),
          maxrisk_idFilter: $('#Maxrisk_idFilterId').val(),
          minsubmission_dateFilter: getDateFilter($('#Minsubmission_dateFilterId')),
          maxsubmission_dateFilter: getMaxDateFilter($('#Maxsubmission_dateFilterId')),
          minlast_updateFilter: getDateFilter($('#Minlast_updateFilterId')),
          maxlast_updateFilter: getMaxDateFilter($('#Maxlast_updateFilterId')),
          minplanning_strategyFilter: $('#Minplanning_strategyFilterId').val(),
          maxplanning_strategyFilter: $('#Maxplanning_strategyFilterId').val(),
          minmitigation_effortFilter: $('#Minmitigation_effortFilterId').val(),
          maxmitigation_effortFilter: $('#Maxmitigation_effortFilterId').val(),
          minmitigation_costFilter: $('#Minmitigation_costFilterId').val(),
          maxmitigation_costFilter: $('#Maxmitigation_costFilterId').val(),
          minmitigation_ownerFilter: $('#Minmitigation_ownerFilterId').val(),
          maxmitigation_ownerFilter: $('#Maxmitigation_ownerFilterId').val(),
          current_solutionFilter: $('#current_solutionFilterId').val(),
          security_requirementsFilter: $('#security_requirementsFilterId').val(),
          security_recommendationsFilter: $('#security_recommendationsFilterId').val(),
          minsubmitted_byFilter: $('#Minsubmitted_byFilterId').val(),
          maxsubmitted_byFilter: $('#Maxsubmitted_byFilterId').val(),
          minplanning_dateFilter: getDateFilter($('#Minplanning_dateFilterId')),
          maxplanning_dateFilter: getMaxDateFilter($('#Maxplanning_dateFilterId')),
          minmitigation_percentFilter: $('#Minmitigation_percentFilterId').val(),
          maxmitigation_percentFilter: $('#Maxmitigation_percentFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditMitigationModalSaved', function () {
      getMitigations();
    });

    $('#GetMitigationsButton').click(function (e) {
      e.preventDefault();
      getMitigations();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getMitigations();
      }
    });
  });
})();
