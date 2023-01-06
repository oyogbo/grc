using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Areas.App.Models.RiskLevels;
using CCPDemo.Web.Controllers;
using CCPDemo.Authorization;
using CCPDemo.RiskLevels;
using CCPDemo.RiskLevels.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_RiskLevels)]
    public class RiskLevelsController : CCPDemoControllerBase
    {
        private readonly IRiskLevelsAppService _riskLevelsAppService;

        public RiskLevelsController(IRiskLevelsAppService riskLevelsAppService)
        {
            _riskLevelsAppService = riskLevelsAppService;

        }

        public ActionResult Index()
        {
            var model = new RiskLevelsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_RiskLevels_Create, AppPermissions.Pages_Administration_RiskLevels_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetRiskLevelForEditOutput getRiskLevelForEditOutput;

            if (id.HasValue)
            {
                getRiskLevelForEditOutput = await _riskLevelsAppService.GetRiskLevelForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getRiskLevelForEditOutput = new GetRiskLevelForEditOutput
                {
                    RiskLevel = new CreateOrEditRiskLevelDto()
                };
            }

            var viewModel = new CreateOrEditRiskLevelModalViewModel()
            {
                RiskLevel = getRiskLevelForEditOutput.RiskLevel,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRiskLevelModal(int id)
        {
            var getRiskLevelForViewDto = await _riskLevelsAppService.GetRiskLevelForView(id);

            var model = new RiskLevelViewModel()
            {
                RiskLevel = getRiskLevelForViewDto.RiskLevel
            };

            return PartialView("_ViewRiskLevelModal", model);
        }

    }
}