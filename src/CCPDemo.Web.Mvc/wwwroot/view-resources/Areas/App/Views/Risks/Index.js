(function () {
  $(function () {
    var _$risksTable = $('#RisksTable');
    var _risksService = abp.services.app.risks;

      $('.date-picker').flatpickr({
      locale: abp.localization.currentLanguage.name,
      format: 'L',
    });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Risks.Create'),
      edit: abp.auth.hasPermission('Pages.Risks.Edit'),
      delete: abp.auth.hasPermission('Pages.Risks.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Risks/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Risks/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditRiskModal',
    });

      var _ermTransferRiskModal = new app.ModalManager({
          viewUrl: abp.appPath + 'App/Risks/ErmTransferRiskModal',
          scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Risks/_ErmTransferRiskModal.js',
          modalClass: 'ErmTransferRiskModal',
      });

      var _ermUpDownRiskModal = new app.ModalManager({
          viewUrl: abp.appPath + 'App/Risks/ErmUpDownRiskModal',
          scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Risks/_ErmUpDownRiskModal.js',
          modalClass: 'ErmUpDownRiskModal',
      });

      var _ermCloseRiskModal = new app.ModalManager({
          viewUrl: abp.appPath + 'App/Risks/ErmCloseRiskModal',
          scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Risks/_ErmCloseRiskModal.js',
          modalClass: 'ErmCloseRiskModal',
      });

    var _viewRiskModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Risks/ViewriskModal',
      modalClass: 'ViewRiskModal',
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

    var dataTable = _$risksTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _risksService.getAll,
        inputFilter: function () {
          return {
            filter: $('#RisksTableFilter').val(),
            riskTypeFilter: $('#RiskTypeFilterId').val(),
            riskSummaryFilter: $('#RiskSummaryFilterId').val(),
            departmentFilter: $('#DepartmentFilterId').val(),
            riskOwnerFilter: $('#RiskOwnerFilterId').val(),
            existingControlFilter: $('#ExistingControlFilterId').val(),
            eRMCommentFilter: $('#ERMCommentFilterId').val(),
            actionPlanFilter: $('#ActionPlanFilterId').val(),
            riskOwnerCommentFilter: $('#RiskOwnerCommentFilterId').val(),
            statusFilter: $('#StatusFilterId').val(),
            ratingFilter: $('#RatingFilterId').val(),
            targetDateFilter: $('#TargetDateFilterId').val(),
            actualClosureDateFilter: $('#ActualClosureDateFilterId').val(),
            riskAcceptedFilter: $('#RiskAcceptedFilterId').val(),
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
                  _viewRiskModal.open({ id: data.record.risk.id });
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.risk.id });
                },
                },
                {
                    text: 'Transfer Risk',
                    iconStyle: 'far fa-paper-plane ml-2 mr-2',
                    visible: function () {
                        return _permissions.create;
                    },
                    action: function (data) {
                        _ermTransferRiskModal.open({ id: data.record.risk.id });
                    },
                },
                {
                    text: 'Downgrade/Upgrade Risk',
                    iconStyle: 'fas fa-arrows-alt-v ml-2 mr-2',
                    visible: function () {
                        return _permissions.create;
                    },
                    action: function (data) {
                        _ermUpDownRiskModal.open({ id: data.record.risk.id });
                    },
                },
                {
                    text: 'Close Risk',
                    iconStyle: 'fas fa-power-off ml-2 mr-2',
                    visible: function () {
                        return _permissions.create;
                    },
                    action: function (data) {
                        _ermCloseRiskModal.open({ id: data.record.risk.id });
                    },
                },
              {
                text: app.localize('Delete'),
                iconStyle: 'far fa-trash-alt mr-2',
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteRisk(data.record.risk);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'risk.riskType',
          name: 'riskType',
        },
        {
          targets: 3,
          data: 'risk.riskSummary',
          name: 'riskSummary',
        },
        {
          targets: 4,
          data: 'risk.department',
          name: 'department',
        },
        {
          targets: 5,
          data: 'risk.riskOwner',
          name: 'riskOwner',
        },
        {
          targets: 6,
          data: 'risk.existingControl',
          name: 'existingControl',
        },
        {
          targets: 7,
          data: 'risk.ermComment',
          name: 'ermComment',
        },
        {
          targets: 8,
          data: 'risk.actionPlan',
          name: 'actionPlan',
        },
        {
          targets: 9,
          data: 'risk.riskOwnerComment',
          name: 'riskOwnerComment',
        },
        {
          targets: 10,
          data: 'risk.status',
          name: 'status',
        },
        {
          targets: 11,
          data: 'risk.rating',
          name: 'rating',
        },
        {
          targets: 12,
          data: 'risk.targetDate',
          name: 'targetDate',
        },
        {
          targets: 13,
          data: 'risk.actualClosureDate',
          name: 'actualClosureDate',
        },
        {
          targets: 14,
          data: 'risk.riskAccepted',
          name: 'riskAccepted',
          render: function (riskAccepted) {
            if (riskAccepted) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
      ],
    });

    function getRisks() {
      dataTable.ajax.reload();
    }

    function deleteRisk(risk) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _risksService
            .delete({
              id: risk.id,
            })
            .done(function () {
              getRisks(true);
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

    $('#CreateNewRiskButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _risksService
        .getRisksToExcel({
          filter: $('#RisksTableFilter').val(),
          riskTypeFilter: $('#RiskTypeFilterId').val(),
          riskSummaryFilter: $('#RiskSummaryFilterId').val(),
          departmentFilter: $('#DepartmentFilterId').val(),
          riskOwnerFilter: $('#RiskOwnerFilterId').val(),
          existingControlFilter: $('#ExistingControlFilterId').val(),
          eRMCommentFilter: $('#ERMCommentFilterId').val(),
          actionPlanFilter: $('#ActionPlanFilterId').val(),
          riskOwnerCommentFilter: $('#RiskOwnerCommentFilterId').val(),
          statusFilter: $('#StatusFilterId').val(),
          ratingFilter: $('#RatingFilterId').val(),
          targetDateFilter: $('#TargetDateFilterId').val(),
          actualClosureDateFilter: $('#ActualClosureDateFilterId').val(),
          riskAcceptedFilter: $('#RiskAcceptedFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditRiskModalSaved', function () {
      getRisks();
    });

    $('#GetRisksButton').click(function (e) {
      e.preventDefault();
      getRisks();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getRisks();
      }
    });
  });
})();
