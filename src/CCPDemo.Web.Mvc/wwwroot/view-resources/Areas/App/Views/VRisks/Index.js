(function () {
  $(function () {
    var _$vRisksTable = $('#VRisksTable');
    var _vRisksService = abp.services.app.vRisks;

    //$('.date-picker').datetimepicker({
    //  locale: abp.localization.currentLanguage.name,
    //  format: 'L',
    //});

    var _permissions = {
      create: abp.auth.hasPermission('Pages.VRisks.Create'),
      edit: abp.auth.hasPermission('Pages.VRisks.Edit'),
      delete: abp.auth.hasPermission('Pages.VRisks.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/VRisks/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/VRisks/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditVRiskModal',
    });

    var _viewVRiskModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/VRisks/ViewvRiskModal',
      modalClass: 'ViewVRiskModal',
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

    var dataTable = _$vRisksTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _vRisksService.getAll,
        inputFilter: function () {
          return {
            filter: $('#VRisksTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
            descriptionFilter: $('#DescriptionFilterId').val(),
            departmentFilter: $('#DepartmentFilterId').val(),
            riskOwnerFilter: $('#RiskOwnerFilterId').val(),
            resolutionTimeLineFilter: $('#ResolutionTimeLineFilterId').val(),
            eRMCommentFilter: $('#ERMCommentFilterId').val(),
            riskOwnerCommentFilter: $('#RiskOwnerCommentFilterId').val(),
            statusFilter: $('#StatusFilterId').val(),
            actualClosureDateFilter: $('#ActualClosureDateFilterId').val(),
            mitigationDateFilter: $('#MitigationDateFilterId').val(),
            acceptRiskFilter: $('#AcceptRiskFilterId').val(),
            acceptanceDateFilter: $('#AcceptanceDateFilterId').val(),
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
                  _viewVRiskModal.open({ id: data.record.vRisk.id });
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.vRisk.id });
                },
              },
              {
                text: app.localize('Delete'),
                iconStyle: 'far fa-trash-alt mr-2',
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteVRisk(data.record.vRisk);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'vRisk.name',
          name: 'name',
        },
        {
          targets: 3,
          data: 'vRisk.description',
          name: 'description',
        },
        {
          targets: 4,
          data: 'vRisk.department',
          name: 'department',
        },
        {
          targets: 5,
          data: 'vRisk.riskOwner',
          name: 'riskOwner',
        },
        {
          targets: 6,
          data: 'vRisk.resolutionTimeLine',
          name: 'resolutionTimeLine',
        },
        {
          targets: 7,
          data: 'vRisk.ermComment',
          name: 'ermComment',
        },
        {
          targets: 8,
          data: 'vRisk.riskOwnerComment',
          name: 'riskOwnerComment',
        },
        {
          targets: 9,
          data: 'vRisk.status',
          name: 'status',
        },
        {
          targets: 10,
          data: 'vRisk.actualClosureDate',
          name: 'actualClosureDate',
        },
        {
          targets: 11,
          data: 'vRisk.mitigationDate',
          name: 'mitigationDate',
        },
        {
          targets: 12,
          data: 'vRisk.acceptRisk',
          name: 'acceptRisk',
          render: function (acceptRisk) {
            if (acceptRisk) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 13,
          data: 'vRisk.acceptanceDate',
          name: 'acceptanceDate',
        },
      ],
    });

    function getVRisks() {
      dataTable.ajax.reload();
    }

    function deleteVRisk(vRisk) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _vRisksService
            .delete({
              id: vRisk.id,
            })
            .done(function () {
              getVRisks(true);
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

    $('#CreateNewVRiskButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _vRisksService
        .getVRisksToExcel({
          filter: $('#VRisksTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
          descriptionFilter: $('#DescriptionFilterId').val(),
          departmentFilter: $('#DepartmentFilterId').val(),
          riskOwnerFilter: $('#RiskOwnerFilterId').val(),
          resolutionTimeLineFilter: $('#ResolutionTimeLineFilterId').val(),
          eRMCommentFilter: $('#ERMCommentFilterId').val(),
          riskOwnerCommentFilter: $('#RiskOwnerCommentFilterId').val(),
          statusFilter: $('#StatusFilterId').val(),
          actualClosureDateFilter: $('#ActualClosureDateFilterId').val(),
          mitigationDateFilter: $('#MitigationDateFilterId').val(),
          acceptRiskFilter: $('#AcceptRiskFilterId').val(),
          acceptanceDateFilter: $('#AcceptanceDateFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditVRiskModalSaved', function () {
      getVRisks();
    });

    $('#GetVRisksButton').click(function (e) {
      e.preventDefault();
      getVRisks();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getVRisks();
      }
    });
  });
})();
