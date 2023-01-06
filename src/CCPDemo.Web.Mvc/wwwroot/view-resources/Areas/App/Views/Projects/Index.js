(function () {
  $(function () {
    var _$projectsTable = $('#ProjectsTable');
    var _projectsService = abp.services.app.projects;

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
        getProjects();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getProjects();
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
        getProjects();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getProjects();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.Projects.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.Projects.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.Projects.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Projects/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Projects/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditProjectModal',
    });

    var _viewProjectModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Projects/ViewprojectModal',
      modalClass: 'ViewProjectModal',
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

    var dataTable = _$projectsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _projectsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#ProjectsTableFilter').val(),
            minValueFilter: $('#MinValueFilterId').val(),
            maxValueFilter: $('#MaxValueFilterId').val(),
            nameFilter: $('#NameFilterId').val(),
            minDueDateFilter: getDateFilter($('#MinDueDateFilterId')),
            maxDueDateFilter: getMaxDateFilter($('#MaxDueDateFilterId')),
            minConsultantIdFilter: $('#MinConsultantIdFilterId').val(),
            maxConsultantIdFilter: $('#MaxConsultantIdFilterId').val(),
            minBusinessOwnerIdFilter: $('#MinBusinessOwnerIdFilterId').val(),
            maxBusinessOwnerIdFilter: $('#MaxBusinessOwnerIdFilterId').val(),
            minDataClassificationIdFilter: $('#MinDataClassificationIdFilterId').val(),
            maxDataClassificationIdFilter: $('#MaxDataClassificationIdFilterId').val(),
            minOrderFilter: $('#MinOrderFilterId').val(),
            maxOrderFilter: $('#MaxOrderFilterId').val(),
            minstatusFilter: $('#MinstatusFilterId').val(),
            maxstatusFilter: $('#MaxstatusFilterId').val(),
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
                  _viewProjectModal.open({ id: data.record.project.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.project.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteProject(data.record.project);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'project.value',
          name: 'value',
        },
        {
          targets: 3,
          data: 'project.name',
          name: 'name',
        },
        {
          targets: 4,
          data: 'project.dueDate',
          name: 'dueDate',
          render: function (dueDate) {
            if (dueDate) {
              return moment(dueDate).format('L');
            }
            return '';
          },
        },
        {
          targets: 5,
          data: 'project.consultantId',
          name: 'consultantId',
        },
        {
          targets: 6,
          data: 'project.businessOwnerId',
          name: 'businessOwnerId',
        },
        {
          targets: 7,
          data: 'project.dataClassificationId',
          name: 'dataClassificationId',
        },
        {
          targets: 8,
          data: 'project.order',
          name: 'order',
        },
        {
          targets: 9,
          data: 'project.status',
          name: 'status',
        },
      ],
    });

    function getProjects() {
      dataTable.ajax.reload();
    }

    function deleteProject(project) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _projectsService
            .delete({
              id: project.id,
            })
            .done(function () {
              getProjects(true);
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

    $('#CreateNewProjectButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _projectsService
        .getProjectsToExcel({
          filter: $('#ProjectsTableFilter').val(),
          minValueFilter: $('#MinValueFilterId').val(),
          maxValueFilter: $('#MaxValueFilterId').val(),
          nameFilter: $('#NameFilterId').val(),
          minDueDateFilter: getDateFilter($('#MinDueDateFilterId')),
          maxDueDateFilter: getMaxDateFilter($('#MaxDueDateFilterId')),
          minConsultantIdFilter: $('#MinConsultantIdFilterId').val(),
          maxConsultantIdFilter: $('#MaxConsultantIdFilterId').val(),
          minBusinessOwnerIdFilter: $('#MinBusinessOwnerIdFilterId').val(),
          maxBusinessOwnerIdFilter: $('#MaxBusinessOwnerIdFilterId').val(),
          minDataClassificationIdFilter: $('#MinDataClassificationIdFilterId').val(),
          maxDataClassificationIdFilter: $('#MaxDataClassificationIdFilterId').val(),
          minOrderFilter: $('#MinOrderFilterId').val(),
          maxOrderFilter: $('#MaxOrderFilterId').val(),
          minstatusFilter: $('#MinstatusFilterId').val(),
          maxstatusFilter: $('#MaxstatusFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditProjectModalSaved', function () {
      getProjects();
    });

    $('#GetProjectsButton').click(function (e) {
      e.preventDefault();
      getProjects();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getProjects();
      }
    });

    $('.reload-on-change').change(function (e) {
      getProjects();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getProjects();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getProjects();
    });
  });
})();
