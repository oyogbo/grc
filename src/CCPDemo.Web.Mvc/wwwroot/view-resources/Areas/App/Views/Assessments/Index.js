(function () {
  $(function () {
    var _$assessmentsTable = $('#AssessmentsTable');
    var _assessmentsService = abp.services.app.assessments;

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
        getAssessments();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getAssessments();
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
        getAssessments();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getAssessments();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Assessments.Create'),
      edit: abp.auth.hasPermission('Pages.Assessments.Edit'),
      delete: abp.auth.hasPermission('Pages.Assessments.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Assessments/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Assessments/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditAssessmentModal',
    });

    var _viewAssessmentModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Assessments/ViewassessmentModal',
      modalClass: 'ViewAssessmentModal',
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

    var dataTable = _$assessmentsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _assessmentsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#AssessmentsTableFilter').val(),
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
                  _viewAssessmentModal.open({ id: data.record.assessment.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.assessment.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteAssessment(data.record.assessment);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'assessment.name',
          name: 'name',
        },
      ],
    });

    function getAssessments() {
      dataTable.ajax.reload();
    }

    function deleteAssessment(assessment) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _assessmentsService
            .delete({
              id: assessment.id,
            })
            .done(function () {
              getAssessments(true);
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

    $('#CreateNewAssessmentButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _assessmentsService
        .getAssessmentsToExcel({
          filter: $('#AssessmentsTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditAssessmentModalSaved', function () {
      getAssessments();
    });

    $('#GetAssessmentsButton').click(function (e) {
      e.preventDefault();
      getAssessments();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getAssessments();
      }
    });

    $('.reload-on-change').change(function (e) {
      getAssessments();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getAssessments();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getAssessments();
    });
  });
})();
