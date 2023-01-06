using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Areas.App.Models.RiskModels;
using CCPDemo.Web.Controllers;
using CCPDemo.Authorization;
using CCPDemo.RiskModels;
using CCPDemo.RiskModels.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_RiskModels)]
    public class RiskModelsController : CCPDemoControllerBase
    {
        private readonly IRiskModelsAppService _riskModelsAppService;

        public RiskModelsController(IRiskModelsAppService riskModelsAppService)
        {
            _riskModelsAppService = riskModelsAppService;

        }

        public ActionResult Index()
        {
            var model = new RiskModelsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_RiskModels_Create, AppPermissions.Pages_Administration_RiskModels_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetRiskModelForEditOutput getRiskModelForEditOutput;

            if (id.HasValue)
            {
                getRiskModelForEditOutput = await _riskModelsAppService.GetRiskModelForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getRiskModelForEditOutput = new GetRiskModelForEditOutput
                {
                    RiskModel = new CreateOrEditRiskModelDto()
                };
            }

            var viewModel = new CreateOrEditRiskModelModalViewModel()
            {
                RiskModel = getRiskModelForEditOutput.RiskModel,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRiskModelModal(int id)
        {
            var getRiskModelForViewDto = await _riskModelsAppService.GetRiskModelForView(id);

            var model = new RiskModelViewModel()
            {
                RiskModel = getRiskModelForViewDto.RiskModel
            };

            return PartialView("_ViewRiskModelModal", model);
        }

    }
}