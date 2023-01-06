using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Areas.App.Models.Closures;
using CCPDemo.Web.Controllers;
using CCPDemo.Authorization;
using CCPDemo.Closures;
using CCPDemo.Closures.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Closures)]
    public class ClosuresController : CCPDemoControllerBase
    {
        private readonly IClosuresAppService _closuresAppService;

        public ClosuresController(IClosuresAppService closuresAppService)
        {
            _closuresAppService = closuresAppService;

        }

        public ActionResult Index()
        {
            var model = new ClosuresViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Closures_Create, AppPermissions.Pages_Administration_Closures_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetClosureForEditOutput getClosureForEditOutput;

            if (id.HasValue)
            {
                getClosureForEditOutput = await _closuresAppService.GetClosureForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getClosureForEditOutput = new GetClosureForEditOutput
                {
                    Closure = new CreateOrEditClosureDto()
                };
                getClosureForEditOutput.Closure.ClosureDate = DateTime.Now;
            }

            var viewModel = new CreateOrEditClosureModalViewModel()
            {
                Closure = getClosureForEditOutput.Closure,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewClosureModal(int id)
        {
            var getClosureForViewDto = await _closuresAppService.GetClosureForView(id);

            var model = new ClosureViewModel()
            {
                Closure = getClosureForViewDto.Closure
            };

            return PartialView("_ViewClosureModal", model);
        }

    }
}