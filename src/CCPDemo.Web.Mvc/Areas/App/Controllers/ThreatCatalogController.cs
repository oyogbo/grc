using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Areas.App.Models.ThreatCatalog;
using CCPDemo.Web.Controllers;
using CCPDemo.Authorization;
using CCPDemo.ThreatCatalogs;
using CCPDemo.ThreatCatalogs.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_ThreatCatalog)]
    public class ThreatCatalogController : CCPDemoControllerBase
    {
        private readonly IThreatCatalogAppService _threatCatalogAppService;

        public ThreatCatalogController(IThreatCatalogAppService threatCatalogAppService)
        {
            _threatCatalogAppService = threatCatalogAppService;

        }

        public ActionResult Index()
        {
            var model = new ThreatCatalogViewModel();

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ThreatCatalog_Create, AppPermissions.Pages_Administration_ThreatCatalog_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetThreatCatalogForEditOutput getThreatCatalogForEditOutput;

            if (id.HasValue)
            {
                getThreatCatalogForEditOutput = await _threatCatalogAppService.GetThreatCatalogForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getThreatCatalogForEditOutput = new GetThreatCatalogForEditOutput
                {
                    ThreatCatalog = new CreateOrEditThreatCatalogDto()
                };
            }

            var viewModel = new CreateOrEditThreatCatalogModalViewModel()
            {
                ThreatCatalog = getThreatCatalogForEditOutput.ThreatCatalog,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewThreatCatalogModal(int id)
        {
            var getThreatCatalogForViewDto = await _threatCatalogAppService.GetThreatCatalogForView(id);

            var model = new ThreatCatalogViewModel()
            {
                ThreatCatalog = getThreatCatalogForViewDto.ThreatCatalog
            };

            return PartialView("_ViewThreatCatalogModal", model);
        }

    }
}