using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Areas.App.Models.RiskGroupings;
using CCPDemo.Web.Controllers;
using CCPDemo.Authorization;
using CCPDemo.RiskGroupings;
using CCPDemo.RiskGroupings.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_RiskGroupings)]
    public class RiskGroupingsController : CCPDemoControllerBase
    {
        private readonly IRiskGroupingsAppService _riskGroupingsAppService;

        public RiskGroupingsController(IRiskGroupingsAppService riskGroupingsAppService)
        {
            _riskGroupingsAppService = riskGroupingsAppService;

        }

        public ActionResult Index()
        {
            var model = new RiskGroupingsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_RiskGroupings_Create, AppPermissions.Pages_Administration_RiskGroupings_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetRiskGroupingForEditOutput getRiskGroupingForEditOutput;

            if (id.HasValue)
            {
                getRiskGroupingForEditOutput = await _riskGroupingsAppService.GetRiskGroupingForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getRiskGroupingForEditOutput = new GetRiskGroupingForEditOutput
                {
                    RiskGrouping = new CreateOrEditRiskGroupingDto()
                };
            }

            var viewModel = new CreateOrEditRiskGroupingModalViewModel()
            {
                RiskGrouping = getRiskGroupingForEditOutput.RiskGrouping,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRiskGroupingModal(int id)
        {
            var getRiskGroupingForViewDto = await _riskGroupingsAppService.GetRiskGroupingForView(id);

            var model = new RiskGroupingViewModel()
            {
                RiskGrouping = getRiskGroupingForViewDto.RiskGrouping
            };

            return PartialView("_ViewRiskGroupingModal", model);
        }

    }
}