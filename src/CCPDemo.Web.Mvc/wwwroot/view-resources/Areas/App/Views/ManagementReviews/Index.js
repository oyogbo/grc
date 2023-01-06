(function () {
  $(function () {
    var _$managementReviewsTable = $('#ManagementReviewsTable');
    var _managementReviewsService = abp.services.app.managementReviews;

    $('.date-picker').datetimepicker({
      locale: abp.localization.currentLanguage.name,
      format: 'L',
    });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.ManagementReviews.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.ManagementReviews.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.ManagementReviews.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/ManagementReviews/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ManagementReviews/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditManagementReviewModal',
    });

    var _viewManagementReviewModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/ManagementReviews/ViewmanagementReviewModal',
      modalClass: 'ViewManagementReviewModal',
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

    var dataTable = _$managementReviewsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _managementReviewsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#ManagementReviewsTableFilter').val(),
            minrisk_idFilter: $('#Minrisk_idFilterId').val(),
            maxrisk_idFilter: $('#Maxrisk_idFilterId').val(),
            minsubmission_dateFilter: getDateFilter($('#Minsubmission_dateFilterId')),
            maxsubmission_dateFilter: getMaxDateFilter($('#Maxsubmission_dateFilterId')),
            minreviewFilter: $('#MinreviewFilterId').val(),
            maxreviewFilter: $('#MaxreviewFilterId').val(),
            minreviewerFilter: $('#MinreviewerFilterId').val(),
            maxreviewerFilter: $('#MaxreviewerFilterId').val(),
            minnext_stepFilter: $('#Minnext_stepFilterId').val(),
            maxnext_stepFilter: $('#Maxnext_stepFilterId').val(),
            commentsFilter: $('#commentsFilterId').val(),
            minnext_reviewFilter: getDateFilter($('#Minnext_reviewFilterId')),
            maxnext_reviewFilter: getMaxDateFilter($('#Maxnext_reviewFilterId')),
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
                  _viewManagementReviewModal.open({ id: data.record.managementReview.id });
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.managementReview.id });
                },
              },
              {
                text: app.localize('Delete'),
                iconStyle: 'far fa-trash-alt mr-2',
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteManagementReview(data.record.managementReview);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'managementReview.risk_id',
          name: 'risk_id',
        },
        {
          targets: 3,
          data: 'managementReview.submission_date',
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
          data: 'managementReview.review',
          name: 'review',
        },
        {
          targets: 5,
          data: 'managementReview.reviewer',
          name: 'reviewer',
        },
        {
          targets: 6,
          data: 'managementReview.next_step',
          name: 'next_step',
        },
        {
          targets: 7,
          data: 'managementReview.comments',
          name: 'comments',
        },
        {
          targets: 8,
          data: 'managementReview.next_review',
          name: 'next_review',
          render: function (next_review) {
            if (next_review) {
              return moment(next_review).format('L');
            }
            return '';
          },
        },
      ],
    });

    function getManagementReviews() {
      dataTable.ajax.reload();
    }

    function deleteManagementReview(managementReview) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _managementReviewsService
            .delete({
              id: managementReview.id,
            })
            .done(function () {
              getManagementReviews(true);
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

    $('#CreateNewManagementReviewButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _managementReviewsService
        .getManagementReviewsToExcel({
          filter: $('#ManagementReviewsTableFilter').val(),
          minrisk_idFilter: $('#Minrisk_idFilterId').val(),
          maxrisk_idFilter: $('#Maxrisk_idFilterId').val(),
          minsubmission_dateFilter: getDateFilter($('#Minsubmission_dateFilterId')),
          maxsubmission_dateFilter: getMaxDateFilter($('#Maxsubmission_dateFilterId')),
          minreviewFilter: $('#MinreviewFilterId').val(),
          maxreviewFilter: $('#MaxreviewFilterId').val(),
          minreviewerFilter: $('#MinreviewerFilterId').val(),
          maxreviewerFilter: $('#MaxreviewerFilterId').val(),
          minnext_stepFilter: $('#Minnext_stepFilterId').val(),
          maxnext_stepFilter: $('#Maxnext_stepFilterId').val(),
          commentsFilter: $('#commentsFilterId').val(),
          minnext_reviewFilter: getDateFilter($('#Minnext_reviewFilterId')),
          maxnext_reviewFilter: getMaxDateFilter($('#Maxnext_reviewFilterId')),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditManagementReviewModalSaved', function () {
      getManagementReviews();
    });

    $('#GetManagementReviewsButton').click(function (e) {
      e.preventDefault();
      getManagementReviews();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getManagementReviews();
      }
    });
  });
})();
