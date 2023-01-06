(function () {
  $(function () {
    var _$departmentsTable = $('#DepartmentsTable');
    var _departmentsService = abp.services.app.departments;

    //$('.date-picker').datetimepicker({
    //  locale: abp.localization.currentLanguage.name,
    //  format: 'L',
    //});

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Departments.Create'),
      edit: abp.auth.hasPermission('Pages.Departments.Edit'),
      delete: abp.auth.hasPermission('Pages.Departments.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Departments/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Departments/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditDepartmentModal',
    });

    var _viewDepartmentModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Departments/ViewdepartmentModal',
      modalClass: 'ViewDepartmentModal',
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

    var dataTable = _$departmentsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _departmentsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#DepartmentsTableFilter').val(),
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
                  _viewDepartmentModal.open({ id: data.record.department.id });
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.department.id });
                },
              },
              {
                text: app.localize('Delete'),
                iconStyle: 'far fa-trash-alt mr-2',
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteDepartment(data.record.department);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'department.name',
          name: 'name',
        },
      ],
    });

    function getDepartments() {
      dataTable.ajax.reload();
    }

    function deleteDepartment(department) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _departmentsService
            .delete({
              id: department.id,
            })
            .done(function () {
              getDepartments(true);
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

    $('#CreateNewDepartmentButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _departmentsService
        .getDepartmentsToExcel({
          filter: $('#DepartmentsTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditDepartmentModalSaved', function () {
      getDepartments();
    });

    $('#GetDepartmentsButton').click(function (e) {
      e.preventDefault();
      getDepartments();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getDepartments();
      }
    });
  });
})();
