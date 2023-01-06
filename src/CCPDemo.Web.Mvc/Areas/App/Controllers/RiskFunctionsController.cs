using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Areas.App.Models.RiskFunctions;
using CCPDemo.Web.Controllers;
using CCPDemo.Authorization;
using CCPDemo.RiskFunctions;
using CCPDemo.RiskFunctions.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_RiskFunctions)]
    public class RiskFunctionsController : CCPDemoControllerBase
    {
        private readonly IRiskFunctionsAppService _riskFunctionsAppService;

        public RiskFunctionsController(IRiskFunctionsAppService riskFunctionsAppService)
        {
            _riskFunctionsAppService = riskFunctionsAppService;

        }

        public ActionResult Index()
        {
            var model = new RiskFunctionsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_RiskFunctions_Create, AppPermissions.Pages_Administration_RiskFunctions_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetRiskFunctionForEditOutput getRiskFunctionForEditOutput;

            if (id.HasValue)
            {
                getRiskFunctionForEditOutput = await _riskFunctionsAppService.GetRiskFunctionForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getRiskFunctionForEditOutput = new GetRiskFunctionForEditOutput
                {
                    RiskFunction = new CreateOrEditRiskFunctionDto()
                };
            }

            var viewModel = new CreateOrEditRiskFunctionModalViewModel()
            {
                RiskFunction = getRiskFunctionForEditOutput.RiskFunction,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRiskFunctionModal(int id)
        {
            var getRiskFunctionForViewDto = await _riskFunctionsAppService.GetRiskFunctionForView(id);

            var model = new RiskFunctionViewModel()
            {
                RiskFunction = getRiskFunctionForViewDto.RiskFunction
            };

            return PartialView("_ViewRiskFunctionModal", model);
        }

    }
}