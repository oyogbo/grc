using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Areas.App.Models.Status;
using CCPDemo.Web.Controllers;
using CCPDemo.Authorization;
using CCPDemo.RiskStatus;
using CCPDemo.RiskStatus.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Status)]
    public class StatusController : CCPDemoControllerBase
    {
        private readonly IStatusAppService _statusAppService;

        public StatusController(IStatusAppService statusAppService)
        {
            _statusAppService = statusAppService;

        }

        public ActionResult Index()
        {
            var model = new StatusViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Status_Create, AppPermissions.Pages_Administration_Status_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetStatusForEditOutput getStatusForEditOutput;

            if (id.HasValue)
            {
                getStatusForEditOutput = await _statusAppService.GetStatusForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getStatusForEditOutput = new GetStatusForEditOutput
                {
                    Status = new CreateOrEditStatusDto()
                };
            }

            var viewModel = new CreateOrEditStatusModalViewModel()
            {
                Status = getStatusForEditOutput.Status,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewStatusModal(int id)
        {
            var getStatusForViewDto = await _statusAppService.GetStatusForView(id);

            var model = new StatusViewModel()
            {
                Status = getStatusForViewDto.Status
            };

            return PartialView("_ViewStatusModal", model);
        }

    }
}