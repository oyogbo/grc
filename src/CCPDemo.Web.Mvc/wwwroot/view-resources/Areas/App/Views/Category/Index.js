(function () {
  $(function () {
    var _$categoryTable = $('#CategoryTable');
    var _categoryService = abp.services.app.category;

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
        getCategory();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getCategory();
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
        getCategory();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getCategory();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Administration.Category.Create'),
      edit: abp.auth.hasPermission('Pages.Administration.Category.Edit'),
      delete: abp.auth.hasPermission('Pages.Administration.Category.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Category/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Category/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditCategoryModal',
    });

    var _viewCategoryModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Category/ViewcategoryModal',
      modalClass: 'ViewCategoryModal',
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

    var dataTable = _$categoryTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _categoryService.getAll,
        inputFilter: function () {
          return {
            filter: $('#CategoryTableFilter').val(),
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
                  _viewCategoryModal.open({ id: data.record.category.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.category.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteCategory(data.record.category);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'category.value',
          name: 'value',
        },
        {
          targets: 3,
          data: 'category.name',
          name: 'name',
        },
      ],
    });

    function getCategory() {
      dataTable.ajax.reload();
    }

    function deleteCategory(category) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _categoryService
            .delete({
              id: category.id,
            })
            .done(function () {
              getCategory(true);
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

    $('#CreateNewCategoryButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _categoryService
        .getCategoryToExcel({
          filter: $('#CategoryTableFilter').val(),
          minValueFilter: $('#MinValueFilterId').val(),
          maxValueFilter: $('#MaxValueFilterId').val(),
          nameFilter: $('#NameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditCategoryModalSaved', function () {
      getCategory();
    });

    $('#GetCategoryButton').click(function (e) {
      e.preventDefault();
      getCategory();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getCategory();
      }
    });

    $('.reload-on-change').change(function (e) {
      getCategory();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getCategory();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getCategory();
    });
  });
})();
