using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Areas.App.Models.Departments;
using CCPDemo.Web.Controllers;
using CCPDemo.Authorization;
using CCPDemo.Departments;
using CCPDemo.Departments.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Departments)]
    public class DepartmentsController : CCPDemoControllerBase
    {
        private readonly IDepartmentsAppService _departmentsAppService;

        public DepartmentsController(IDepartmentsAppService departmentsAppService)
        {
            _departmentsAppService = departmentsAppService;

        }

        public ActionResult Index()
        {
            var model = new DepartmentsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Departments_Create, AppPermissions.Pages_Departments_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetDepartmentForEditOutput getDepartmentForEditOutput;

            if (id.HasValue)
            {
                getDepartmentForEditOutput = await _departmentsAppService.GetDepartmentForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getDepartmentForEditOutput = new GetDepartmentForEditOutput
                {
                    Department = new CreateOrEditDepartmentDto()
                };
            }

            var viewModel = new CreateOrEditDepartmentModalViewModel()
            {
                Department = getDepartmentForEditOutput.Department,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewDepartmentModal(int id)
        {
            var getDepartmentForViewDto = await _departmentsAppService.GetDepartmentForView(id);

            var model = new DepartmentViewModel()
            {
                Department = getDepartmentForViewDto.Department
            };

            return PartialView("_ViewDepartmentModal", model);
        }

    }
}