using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Areas.App.Models.RiskRatings;
using CCPDemo.Web.Controllers;
using CCPDemo.Authorization;
using CCPDemo.RiskRatings;
using CCPDemo.RiskRatings.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_RiskRatings)]
    public class RiskRatingsController : CCPDemoControllerBase
    {
        private readonly IRiskRatingsAppService _riskRatingsAppService;

        public RiskRatingsController(IRiskRatingsAppService riskRatingsAppService)
        {
            _riskRatingsAppService = riskRatingsAppService;

        }

        public ActionResult Index()
        {
            var model = new RiskRatingsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_RiskRatings_Create, AppPermissions.Pages_Administration_RiskRatings_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetRiskRatingForEditOutput getRiskRatingForEditOutput;

            if (id.HasValue)
            {
                getRiskRatingForEditOutput = await _riskRatingsAppService.GetRiskRatingForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getRiskRatingForEditOutput = new GetRiskRatingForEditOutput
                {
                    RiskRating = new CreateOrEditRiskRatingDto()
                };
            }

            var viewModel = new CreateOrEditRiskRatingModalViewModel()
            {
                RiskRating = getRiskRatingForEditOutput.RiskRating,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRiskRatingModal(int id)
        {
            var getRiskRatingForViewDto = await _riskRatingsAppService.GetRiskRatingForView(id);

            var model = new RiskRatingViewModel()
            {
                RiskRating = getRiskRatingForViewDto.RiskRating
            };

            return PartialView("_ViewRiskRatingModal", model);
        }

    }
}