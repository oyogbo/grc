using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Areas.App.Models.CloseReasons;
using CCPDemo.Web.Controllers;
using CCPDemo.Authorization;
using CCPDemo.RiskCloseReason;
using CCPDemo.RiskCloseReason.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_CloseReasons)]
    public class CloseReasonsController : CCPDemoControllerBase
    {
        private readonly ICloseReasonsAppService _closeReasonsAppService;

        public CloseReasonsController(ICloseReasonsAppService closeReasonsAppService)
        {
            _closeReasonsAppService = closeReasonsAppService;

        }

        public ActionResult Index()
        {
            var model = new CloseReasonsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_CloseReasons_Create, AppPermissions.Pages_Administration_CloseReasons_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetCloseReasonForEditOutput getCloseReasonForEditOutput;

            if (id.HasValue)
            {
                getCloseReasonForEditOutput = await _closeReasonsAppService.GetCloseReasonForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getCloseReasonForEditOutput = new GetCloseReasonForEditOutput
                {
                    CloseReason = new CreateOrEditCloseReasonDto()
                };
            }

            var viewModel = new CreateOrEditCloseReasonModalViewModel()
            {
                CloseReason = getCloseReasonForEditOutput.CloseReason,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewCloseReasonModal(int id)
        {
            var getCloseReasonForViewDto = await _closeReasonsAppService.GetCloseReasonForView(id);

            var model = new CloseReasonViewModel()
            {
                CloseReason = getCloseReasonForViewDto.CloseReason
            };

            return PartialView("_ViewCloseReasonModal", model);
        }

    }
}