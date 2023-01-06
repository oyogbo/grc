using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Areas.App.Models.RiskTypes;
using CCPDemo.Web.Controllers;
using CCPDemo.Authorization;
using CCPDemo.RiskTypes;
using CCPDemo.RiskTypes.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_RiskTypes)]
    public class RiskTypesController : CCPDemoControllerBase
    {
        private readonly IRiskTypesAppService _riskTypesAppService;

        public RiskTypesController(IRiskTypesAppService riskTypesAppService)
        {
            _riskTypesAppService = riskTypesAppService;

        }

        public ActionResult Index()
        {
            var model = new RiskTypesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_RiskTypes_Create, AppPermissions.Pages_Administration_RiskTypes_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetRiskTypeForEditOutput getRiskTypeForEditOutput;

            if (id.HasValue)
            {
                getRiskTypeForEditOutput = await _riskTypesAppService.GetRiskTypeForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getRiskTypeForEditOutput = new GetRiskTypeForEditOutput
                {
                    RiskType = new CreateOrEditRiskTypeDto()
                };
            }

            var viewModel = new CreateOrEditRiskTypeModalViewModel()
            {
                RiskType = getRiskTypeForEditOutput.RiskType,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRiskTypeModal(int id)
        {
            var getRiskTypeForViewDto = await _riskTypesAppService.GetRiskTypeForView(id);

            var model = new RiskTypeViewModel()
            {
                RiskType = getRiskTypeForViewDto.RiskType
            };

            return PartialView("_ViewRiskTypeModal", model);
        }

    }
}