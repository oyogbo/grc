using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Areas.App.Models.ThreatGrouping;
using CCPDemo.Web.Controllers;
using CCPDemo.Authorization;
using CCPDemo.ThreatGroupings;
using CCPDemo.ThreatGroupings.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_ThreatGrouping)]
    public class ThreatGroupingController : CCPDemoControllerBase
    {
        private readonly IThreatGroupingAppService _threatGroupingAppService;

        public ThreatGroupingController(IThreatGroupingAppService threatGroupingAppService)
        {
            _threatGroupingAppService = threatGroupingAppService;

        }

        public ActionResult Index()
        {
            var model = new ThreatGroupingViewModel();

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ThreatGrouping_Create, AppPermissions.Pages_Administration_ThreatGrouping_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetThreatGroupingForEditOutput getThreatGroupingForEditOutput;

            if (id.HasValue)
            {
                getThreatGroupingForEditOutput = await _threatGroupingAppService.GetThreatGroupingForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getThreatGroupingForEditOutput = new GetThreatGroupingForEditOutput
                {
                    ThreatGrouping = new CreateOrEditThreatGroupingDto()
                };
            }

            var viewModel = new CreateOrEditThreatGroupingModalViewModel()
            {
                ThreatGrouping = getThreatGroupingForEditOutput.ThreatGrouping,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewThreatGroupingModal(int id)
        {
            var getThreatGroupingForViewDto = await _threatGroupingAppService.GetThreatGroupingForView(id);

            var model = new ThreatGroupingViewModel()
            {
                ThreatGrouping = getThreatGroupingForViewDto.ThreatGrouping
            };

            return PartialView("_ViewThreatGroupingModal", model);
        }

    }
}