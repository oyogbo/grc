using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Areas.App.Models.Employees;
using CCPDemo.Web.Controllers;
using CCPDemo.Authorization;
using CCPDemo.Employee;
using CCPDemo.Employee.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Employees)]
    public class EmployeesController : CCPDemoControllerBase
    {
        private readonly IEmployeesAppService _employeesAppService;

        public EmployeesController(IEmployeesAppService employeesAppService)
        {
            _employeesAppService = employeesAppService;

        }

        public ActionResult Index()
        {
            var model = new EmployeesViewModel();

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Employees_Create, AppPermissions.Pages_Employees_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetEmployeesForEditOutput getEmployeesForEditOutput;

            if (id.HasValue)
            {
                getEmployeesForEditOutput = await _employeesAppService.GetEmployeesForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getEmployeesForEditOutput = new GetEmployeesForEditOutput
                {
                    Employees = new CreateOrEditEmployeesDto()
                };
                getEmployeesForEditOutput.Employees.BirthDate = DateTime.Now;
                getEmployeesForEditOutput.Employees.HireDate = DateTime.Now;
            }

            var viewModel = new CreateOrEditEmployeesModalViewModel()
            {
                Employees = getEmployeesForEditOutput.Employees,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewEmployeesModal(int id)
        {
            var getEmployeesForViewDto = await _employeesAppService.GetEmployeesForView(id);

            var model = new EmployeesViewModel()
            {
                Employees = getEmployeesForViewDto.Employees
            };

            return PartialView("_ViewEmployeesModal", model);
        }

    }
}