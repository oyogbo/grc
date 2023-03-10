(function () {
  $(function () {
    var _$reviewTypeTable = $('#ReviewTypeTable');
    var _reviewTypeService = abp.services.app.reviewType;

    $('.date-picker').datetimepicker({
      locale: abp.localization.currentLanguage.name,
      format: 'L',
    });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.ReviewType.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.ReviewType.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.ReviewType.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/ReviewType/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ReviewType/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditReviewTypeModal',
    });

    var _viewReviewTypeModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/ReviewType/ViewreviewTypeModal',
      modalClass: 'ViewReviewTypeModal',
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

    var dataTable = _$reviewTypeTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _reviewTypeService.getAll,
        inputFilter: function () {
          return {
            filter: $('#ReviewTypeTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
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
                  _viewReviewTypeModal.open({ id: data.record.reviewType.id });
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.reviewType.id });
                },
              },
              {
                text: app.localize('Delete'),
                iconStyle: 'far fa-trash-alt mr-2',
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteReviewType(data.record.reviewType);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'reviewType.name',
          name: 'name',
        },
      ],
    });

    function getReviewType() {
      dataTable.ajax.reload();
    }

    function deleteReviewType(reviewType) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _reviewTypeService
            .delete({
              id: reviewType.id,
            })
            .done(function () {
              getReviewType(true);
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

    $('#CreateNewReviewTypeButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _reviewTypeService
        .getReviewTypeToExcel({
          filter: $('#ReviewTypeTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditReviewTypeModalSaved', function () {
      getReviewType();
    });

    $('#GetReviewTypeButton').click(function (e) {
      e.preventDefault();
      getReviewType();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getReviewType();
      }
    });
  });
})();
