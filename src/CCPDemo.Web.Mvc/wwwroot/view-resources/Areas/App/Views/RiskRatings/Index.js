(function () {
  $(function () {
    var _$riskRatingsTable = $('#RiskRatingsTable');
    var _riskRatingsService = abp.services.app.riskRatings;

    //$('.date-picker').datetimepicker({
    //  locale: abp.localization.currentLanguage.name,
    //  format: 'L',
    //});

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.RiskRatings.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.RiskRatings.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.RiskRatings.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RiskRatings/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/RiskRatings/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditRiskRatingModal',
    });

    var _viewRiskRatingModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/RiskRatings/ViewriskRatingModal',
      modalClass: 'ViewRiskRatingModal',
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

    var dataTable = _$riskRatingsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _riskRatingsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#RiskRatingsTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
            colorFilter: $('#ColorFilterId').val(),
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
                  _viewRiskRatingModal.open({ id: data.record.riskRating.id });
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.riskRating.id });
                },
              },
              {
                text: app.localize('Delete'),
                iconStyle: 'far fa-trash-alt mr-2',
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteRiskRating(data.record.riskRating);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'riskRating.name',
          name: 'name',
        },
        {
          targets: 3,
          data: 'riskRating.color',
          name: 'color',
        },
      ],
    });

    function getRiskRatings() {
      dataTable.ajax.reload();
    }

    function deleteRiskRating(riskRating) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _riskRatingsService
            .delete({
              id: riskRating.id,
            })
            .done(function () {
              getRiskRatings(true);
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

    $('#CreateNewRiskRatingButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _riskRatingsService
        .getRiskRatingsToExcel({
          filter: $('#RiskRatingsTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
          colorFilter: $('#ColorFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditRiskRatingModalSaved', function () {
      getRiskRatings();
    });

    $('#GetRiskRatingsButton').click(function (e) {
      e.preventDefault();
      getRiskRatings();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getRiskRatings();
      }
    });
  });
})();
