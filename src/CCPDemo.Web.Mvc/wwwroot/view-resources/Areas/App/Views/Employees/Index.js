(function () {
  $(function () {
    var _$employeesTable = $('#EmployeesTable');
    var _employeesService = abp.services.app.employees;

    $('.date-picker').datetimepicker({
      locale: abp.localization.currentLanguage.name,
      format: 'L',
    });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Employees.Create'),
      edit: abp.auth.hasPermission('Pages.Employees.Edit'),
      delete: abp.auth.hasPermission('Pages.Employees.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Employees/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Employees/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditEmployeesModal',
    });

    var _viewEmployeesModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Employees/ViewemployeesModal',
      modalClass: 'ViewEmployeesModal',
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

    var dataTable = _$employeesTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _employeesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#EmployeesTableFilter').val(),
            minEmployeeIDFilter: $('#MinEmployeeIDFilterId').val(),
            maxEmployeeIDFilter: $('#MaxEmployeeIDFilterId').val(),
            lastNameFilter: $('#LastNameFilterId').val(),
            firstNameFilter: $('#FirstNameFilterId').val(),
            titleFilter: $('#TitleFilterId').val(),
            titleOfCourtesyFilter: $('#TitleOfCourtesyFilterId').val(),
            minBirthDateFilter: getDateFilter($('#MinBirthDateFilterId')),
            maxBirthDateFilter: getMaxDateFilter($('#MaxBirthDateFilterId')),
            minHireDateFilter: getDateFilter($('#MinHireDateFilterId')),
            maxHireDateFilter: getMaxDateFilter($('#MaxHireDateFilterId')),
            addressFilter: $('#AddressFilterId').val(),
            cityFilter: $('#CityFilterId').val(),
            regionFilter: $('#RegionFilterId').val(),
            postalCodeFilter: $('#PostalCodeFilterId').val(),
            countryFilter: $('#CountryFilterId').val(),
            homePhoneFilter: $('#HomePhoneFilterId').val(),
            extensionFilter: $('#ExtensionFilterId').val(),
            photoFilter: $('#PhotoFilterId').val(),
            notesFilter: $('#NotesFilterId').val(),
            minReportsToFilter: $('#MinReportsToFilterId').val(),
            maxReportsToFilter: $('#MaxReportsToFilterId').val(),
            photoPathFilter: $('#PhotoPathFilterId').val(),
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
                  _viewEmployeesModal.open({ id: data.record.employees.id });
                },
              },
              {
                text: app.localize('Edit'),
                iconStyle: 'far fa-edit mr-2',
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.employees.id });
                },
              },
              {
                text: app.localize('Delete'),
                iconStyle: 'far fa-trash-alt mr-2',
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteEmployees(data.record.employees);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'employees.employeeID',
          name: 'employeeID',
        },
        {
          targets: 3,
          data: 'employees.lastName',
          name: 'lastName',
        },
        {
          targets: 4,
          data: 'employees.firstName',
          name: 'firstName',
        },
        {
          targets: 5,
          data: 'employees.title',
          name: 'title',
        },
        {
          targets: 6,
          data: 'employees.titleOfCourtesy',
          name: 'titleOfCourtesy',
        },
        {
          targets: 7,
          data: 'employees.birthDate',
          name: 'birthDate',
          render: function (birthDate) {
            if (birthDate) {
              return moment(birthDate).format('L');
            }
            return '';
          },
        },
        {
          targets: 8,
          data: 'employees.hireDate',
          name: 'hireDate',
          render: function (hireDate) {
            if (hireDate) {
              return moment(hireDate).format('L');
            }
            return '';
          },
        },
        {
          targets: 9,
          data: 'employees.address',
          name: 'address',
        },
        {
          targets: 10,
          data: 'employees.city',
          name: 'city',
        },
        {
          targets: 11,
          data: 'employees.region',
          name: 'region',
        },
        {
          targets: 12,
          data: 'employees.postalCode',
          name: 'postalCode',
        },
        {
          targets: 13,
          data: 'employees.country',
          name: 'country',
        },
        {
          targets: 14,
          data: 'employees.homePhone',
          name: 'homePhone',
        },
        {
          targets: 15,
          data: 'employees.extension',
          name: 'extension',
        },
        {
          targets: 16,
          data: 'employees.photo',
          name: 'photo',
        },
        {
          targets: 17,
          data: 'employees.notes',
          name: 'notes',
        },
        {
          targets: 18,
          data: 'employees.reportsTo',
          name: 'reportsTo',
        },
        {
          targets: 19,
          data: 'employees.photoPath',
          name: 'photoPath',
        },
      ],
    });

    function getEmployees() {
      dataTable.ajax.reload();
    }

    function deleteEmployees(employees) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _employeesService
            .delete({
              id: employees.id,
            })
            .done(function () {
              getEmployees(true);
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

    $('#CreateNewEmployeesButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _employeesService
        .getEmployeesToExcel({
          filter: $('#EmployeesTableFilter').val(),
          minEmployeeIDFilter: $('#MinEmployeeIDFilterId').val(),
          maxEmployeeIDFilter: $('#MaxEmployeeIDFilterId').val(),
          lastNameFilter: $('#LastNameFilterId').val(),
          firstNameFilter: $('#FirstNameFilterId').val(),
          titleFilter: $('#TitleFilterId').val(),
          titleOfCourtesyFilter: $('#TitleOfCourtesyFilterId').val(),
          minBirthDateFilter: getDateFilter($('#MinBirthDateFilterId')),
          maxBirthDateFilter: getMaxDateFilter($('#MaxBirthDateFilterId')),
          minHireDateFilter: getDateFilter($('#MinHireDateFilterId')),
          maxHireDateFilter: getMaxDateFilter($('#MaxHireDateFilterId')),
          addressFilter: $('#AddressFilterId').val(),
          cityFilter: $('#CityFilterId').val(),
          regionFilter: $('#RegionFilterId').val(),
          postalCodeFilter: $('#PostalCodeFilterId').val(),
          countryFilter: $('#CountryFilterId').val(),
          homePhoneFilter: $('#HomePhoneFilterId').val(),
          extensionFilter: $('#ExtensionFilterId').val(),
          photoFilter: $('#PhotoFilterId').val(),
          notesFilter: $('#NotesFilterId').val(),
          minReportsToFilter: $('#MinReportsToFilterId').val(),
          maxReportsToFilter: $('#MaxReportsToFilterId').val(),
          photoPathFilter: $('#PhotoPathFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditEmployeesModalSaved', function () {
      getEmployees();
    });

    $('#GetEmployeesButton').click(function (e) {
      e.preventDefault();
      getEmployees();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getEmployees();
      }
    });
  });
})();
