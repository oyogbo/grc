(function () {

    $('#riskTypeId').focus();

  $(function () {
    var _$risksTable = $('#RisksTable');
      var _risksService = abp.services.app.risks;

      //$.datetimepicker.setDateFormatter({
      //    parseDate: function (date, format) {
      //        var d = moment(date, format);
      //        return d.isValid() ? d.toDate() : false;
      //    },
      //    formatDate: function (date, format) {
      //        return moment(date).format(format);
      //    },
      //    date: function (date) {
      //        return moment(date);
      //    }
      //});


      $('.date-picker').flatpickr({
      defaultDate: null,
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

      var _ermTransferRisk = new app.ModalManager({
          viewUrl: abp.appPath + 'App/Risks/ErmTransferRisk',
          scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Risks/_ErmTransferRisk.js',
          modalClass: 'ErmTransferRisk',
      });
      var _ermUpgradeDowngradeRisk = new app.ModalManager({
          viewUrl: abp.appPath + 'App/Risks/ErmUpgradeOrDowngradeRisk',
          scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Risks/_ErmUpgradeOrDowngradeRisk.js',
          modalClass: 'ErmUpgradeDowngradeRisk',
      });
      var _ermCloseRisk = new app.ModalManager({
          viewUrl: abp.appPath + 'App/Risks/ErmCloseRisk',
          scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Risks/_ErmCloseRisk.js',
          modalClass: 'ErmCloseRisk',
      });

    var _viewRiskModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Risks/ViewriskModal',
      modalClass: 'ViewRiskModal',
    });

    //var getDateFilter = function (element) {
    //  if (element.data('DateTimePicker').date() == null) {
    //    return null;
    //  }
    //  return element.data('DateTimePicker').date().format('YYYY-MM-DDT00:00:00Z');
    //};

    //var getMaxDateFilter = function (element) {
    //  if (element.data('DateTimePicker').date() == null) {
    //    return null;
    //  }
    //  return element.data('DateTimePicker').date().format('YYYY-MM-DDT23:59:59Z');
    //};

    var dataTable = _$risksTable.DataTable({
        paging: true,
        serverSide: true,
        processing: true,
        listAction: {
        ajaxFunction: _risksService.getAll,
        inputFilter: function () {
          return {
            filter: $('#RisksTableFilter').val(),
            summaryFilter: $('#SummaryFilterId').val(),
            existingControlFilter: $('#ExistingControlFilterId').val(),
            eRMRecommendationFilter: $('#ERMRecommendationFilterId').val(),
            actionPlanFilter: $('#ActionPlanFilterId').val(),
            riskOwnerCommentFilter: $('#RiskOwnerCommentFilterId').val(),
            //minTargetDateFilter: getDateFilter($('#MinTargetDateFilterId')),
            //maxTargetDateFilter: getMaxDateFilter($('#MaxTargetDateFilterId')),
            //minActualClosureDateFilter: getDateFilter($('#MinActualClosureDateFilterId')),
            //maxActualClosureDateFilter: getMaxDateFilter($('#MaxActualClosureDateFilterId')),
            //minAcceptanceDateFilter: getDateFilter($('#MinAcceptanceDateFilterId')),
            //maxAcceptanceDateFilter: getMaxDateFilter($('#MaxAcceptanceDateFilterId')),
            riskAcceptedFilter: $('#RiskAcceptedFilterId').val(),
            riskTypeNameFilter: $('#RiskTypeNameFilterId').val(),
            organizationUnitDisplayNameFilter: $('#OrganizationUnitDisplayNameFilterId').val(),
            statusNameFilter: $('#StatusNameFilterId').val(),
            riskRatingNameFilter: $('#RiskRatingNameFilterId').val(),
            userNameFilter: $('#UserNameFilterId').val(),
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
                    iconStyle: 'far fa-edit mr-2',
                    visible: function () {
                        return _permissions.create;
                    },
                    action: function (data) {
                        _ermTransferRisk.open({ id: data.record.risk.id });
                    },
                },
                {
                    text: 'Upgrade/Downgrade Risk',
                    iconStyle: 'far fa-edit mr-2',
                    visible: function () {
                        return _permissions.create;
                    },
                    action: function (data) {
                        _ermUpgradeDowngradeRisk.open({ id: data.record.risk.id });
                    },
                },
                {
                    text: 'Close Risk',
                    iconStyle: 'far fa-edit mr-2',
                    visible: function () {
                        return _permissions.create;
                    },
                    action: function (data) {
                        _ermCloseRisk.open({ id: data.record.risk.id });
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
          data: 'risk.summary',
          name: 'summary',
        },
        {
          targets: 3,
          data: 'risk.existingControl',
          name: 'existingControl',
        },
        {
          targets: 4,
          data: 'risk.ermRecommendation',
          name: 'ermRecommendation',
        },
        {
          targets: 5,
          data: 'risk.actionPlan',
          name: 'actionPlan',
        },
        {
          targets: 6,
          data: 'risk.riskOwnerComment',
          name: 'riskOwnerComment',
        },
        {
          targets: 7,
          data: 'risk.targetDate',
          name: 'targetDate',
          render: function (targetDate) {
            if (targetDate) {
                return moment(targetDate).format('L').replace('01/01/0001','');
            }
            return '';
          },
        },
        {
          targets: 8,
          data: 'risk.actualClosureDate',
          name: 'actualClosureDate',
          render: function (actualClosureDate) {
            if (actualClosureDate) {
                return moment(actualClosureDate).format('L').replace('01/01/0001', '');
            }
            return '';
          },
        },
        {
          targets: 9,
          data: 'risk.acceptanceDate',
          name: 'acceptanceDate',
          render: function (acceptanceDate) {
            if (acceptanceDate) {
                return moment(acceptanceDate).format('L').replace('01/01/0001', '');
            }
            return '';
          },
        },
        {
          targets: 10,
          data: 'risk.riskAccepted',
          name: 'riskAccepted',
          render: function (riskAccepted) {
            if (riskAccepted) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 11,
          data: 'riskTypeName',
          name: 'riskTypeFk.name',
        },
        {
          targets: 12,
          data: 'organizationUnitDisplayName',
          name: 'organizationUnitFk.displayName',
        },
        {
          targets: 13,
          data: 'statusName',
          name: 'statusFk.name',
        },
        {
          targets: 14,
          data: 'riskRatingName',
          name: 'riskRatingFk.name',
        },
        {
          targets: 15,
          data: 'userName',
          name: 'userFk.name',
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
          summaryFilter: $('#SummaryFilterId').val(),
          existingControlFilter: $('#ExistingControlFilterId').val(),
          eRMRecommendationFilter: $('#ERMRecommendationFilterId').val(),
          actionPlanFilter: $('#ActionPlanFilterId').val(),
          riskOwnerCommentFilter: $('#RiskOwnerCommentFilterId').val(),
          //minTargetDateFilter: getDateFilter($('#MinTargetDateFilterId')),
          //maxTargetDateFilter: getMaxDateFilter($('#MaxTargetDateFilterId')),
          //minActualClosureDateFilter: getDateFilter($('#MinActualClosureDateFilterId')),
          //maxActualClosureDateFilter: getMaxDateFilter($('#MaxActualClosureDateFilterId')),
          //minAcceptanceDateFilter: getDateFilter($('#MinAcceptanceDateFilterId')),
          //maxAcceptanceDateFilter: getMaxDateFilter($('#MaxAcceptanceDateFilterId')),
          riskAcceptedFilter: $('#RiskAcceptedFilterId').val(),
          riskTypeNameFilter: $('#RiskTypeNameFilterId').val(),
          organizationUnitDisplayNameFilter: $('#OrganizationUnitDisplayNameFilterId').val(),
          statusNameFilter: $('#StatusNameFilterId').val(),
          riskRatingNameFilter: $('#RiskRatingNameFilterId').val(),
          userNameFilter: $('#UserNameFilterId').val(),
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
