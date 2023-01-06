using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Areas.App.Models.Regulations;
using CCPDemo.Web.Controllers;
using CCPDemo.Authorization;
using CCPDemo.Regulations;
using CCPDemo.Regulations.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Regulations)]
    public class RegulationsController : CCPDemoControllerBase
    {
        private readonly IRegulationsAppService _regulationsAppService;

        public RegulationsController(IRegulationsAppService regulationsAppService)
        {
            _regulationsAppService = regulationsAppService;

        }

        public ActionResult Index()
        {
            var model = new RegulationsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Regulations_Create, AppPermissions.Pages_Administration_Regulations_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetRegulationForEditOutput getRegulationForEditOutput;

            if (id.HasValue)
            {
                getRegulationForEditOutput = await _regulationsAppService.GetRegulationForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getRegulationForEditOutput = new GetRegulationForEditOutput
                {
                    Regulation = new CreateOrEditRegulationDto()
                };
            }

            var viewModel = new CreateOrEditRegulationModalViewModel()
            {
                Regulation = getRegulationForEditOutput.Regulation,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRegulationModal(int id)
        {
            var getRegulationForViewDto = await _regulationsAppService.GetRegulationForView(id);

            var model = new RegulationViewModel()
            {
                Regulation = getRegulationForViewDto.Regulation
            };

            return PartialView("_ViewRegulationModal", model);
        }

    }
}