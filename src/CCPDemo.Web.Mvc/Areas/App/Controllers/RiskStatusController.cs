using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Areas.App.Models.RiskStatus;
using CCPDemo.Web.Controllers;
using CCPDemo.Authorization;
using CCPDemo.RiskStatuses;
using CCPDemo.RiskStatuses.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_RiskStatus)]
    public class RiskStatusController : CCPDemoControllerBase
    {
        private readonly IRiskStatusAppService _riskStatusAppService;

        public RiskStatusController(IRiskStatusAppService riskStatusAppService)
        {
            _riskStatusAppService = riskStatusAppService;

        }

        public ActionResult Index()
        {
            var model = new RiskStatusViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_RiskStatus_Create, AppPermissions.Pages_Administration_RiskStatus_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetRiskStatusForEditOutput getRiskStatusForEditOutput;

            if (id.HasValue)
            {
                getRiskStatusForEditOutput = await _riskStatusAppService.GetRiskStatusForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getRiskStatusForEditOutput = new GetRiskStatusForEditOutput
                {
                    RiskStatus = new CreateOrEditRiskStatusDto()
                };
            }

            var viewModel = new CreateOrEditRiskStatusModalViewModel()
            {
                RiskStatus = getRiskStatusForEditOutput.RiskStatus,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRiskStatusModal(int id)
        {
            var getRiskStatusForViewDto = await _riskStatusAppService.GetRiskStatusForView(id);

            var model = new RiskStatusViewModel()
            {
                RiskStatus = getRiskStatusForViewDto.RiskStatus
            };

            return PartialView("_ViewRiskStatusModal", model);
        }

    }
}