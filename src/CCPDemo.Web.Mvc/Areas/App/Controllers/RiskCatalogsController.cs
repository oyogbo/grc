using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Areas.App.Models.RiskCatalogs;
using CCPDemo.Web.Controllers;
using CCPDemo.Authorization;
using CCPDemo.RiskCatalogs;
using CCPDemo.RiskCatalogs.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_RiskCatalogs)]
    public class RiskCatalogsController : CCPDemoControllerBase
    {
        private readonly IRiskCatalogsAppService _riskCatalogsAppService;

        public RiskCatalogsController(IRiskCatalogsAppService riskCatalogsAppService)
        {
            _riskCatalogsAppService = riskCatalogsAppService;

        }

        public ActionResult Index()
        {
            var model = new RiskCatalogsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_RiskCatalogs_Create, AppPermissions.Pages_Administration_RiskCatalogs_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetRiskCatalogForEditOutput getRiskCatalogForEditOutput;

            if (id.HasValue)
            {
                getRiskCatalogForEditOutput = await _riskCatalogsAppService.GetRiskCatalogForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getRiskCatalogForEditOutput = new GetRiskCatalogForEditOutput
                {
                    RiskCatalog = new CreateOrEditRiskCatalogDto()
                };
            }

            var viewModel = new CreateOrEditRiskCatalogModalViewModel()
            {
                RiskCatalog = getRiskCatalogForEditOutput.RiskCatalog,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRiskCatalogModal(int id)
        {
            var getRiskCatalogForViewDto = await _riskCatalogsAppService.GetRiskCatalogForView(id);

            var model = new RiskCatalogViewModel()
            {
                RiskCatalog = getRiskCatalogForViewDto.RiskCatalog
            };

            return PartialView("_ViewRiskCatalogModal", model);
        }

    }
}