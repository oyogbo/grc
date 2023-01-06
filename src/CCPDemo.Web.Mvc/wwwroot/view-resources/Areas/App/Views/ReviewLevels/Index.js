(function () {
  $(function () {
    var _$reviewLevelsTable = $('#ReviewLevelsTable');
    var _reviewLevelsService = abp.services.app.reviewLevels;

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
        getReviewLevels();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getReviewLevels();
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
        getReviewLevels();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getReviewLevels();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.ReviewLevels.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.ReviewLevels.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.ReviewLevels.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/ReviewLevels/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ReviewLevels/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditReviewLevelModal',
    });

    var _viewReviewLevelModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/ReviewLevels/ViewreviewLevelModal',
      modalClass: 'ViewReviewLevelModal',
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

    var dataTable = _$reviewLevelsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _reviewLevelsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#ReviewLevelsTableFilter').val(),
            minValueFilter: $('#MinValueFilterId').val(),
            maxValueFilter: $('#MaxValueFilterId').val(),
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
                action: function (data) {
                  _viewReviewLevelModal.open({ id: data.record.reviewLevel.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.reviewLevel.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteReviewLevel(data.record.reviewLevel);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'reviewLevel.value',
          name: 'value',
        },
        {
          targets: 3,
          data: 'reviewLevel.name',
          name: 'name',
        },
      ],
    });

    function getReviewLevels() {
      dataTable.ajax.reload();
    }

    function deleteReviewLevel(reviewLevel) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _reviewLevelsService
            .delete({
              id: reviewLevel.id,
            })
            .done(function () {
              getReviewLevels(true);
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

    $('#CreateNewReviewLevelButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _reviewLevelsService
        .getReviewLevelsToExcel({
          filter: $('#ReviewLevelsTableFilter').val(),
          minValueFilter: $('#MinValueFilterId').val(),
          maxValueFilter: $('#MaxValueFilterId').val(),
          nameFilter: $('#NameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditReviewLevelModalSaved', function () {
      getReviewLevels();
    });

    $('#GetReviewLevelsButton').click(function (e) {
      e.preventDefault();
      getReviewLevels();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getReviewLevels();
      }
    });

    $('.reload-on-change').change(function (e) {
      getReviewLevels();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getReviewLevels();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getReviewLevels();
    });
  });
})();
