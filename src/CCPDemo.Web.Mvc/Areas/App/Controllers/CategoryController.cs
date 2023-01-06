using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Areas.App.Models.Category;
using CCPDemo.Web.Controllers;
using CCPDemo.Authorization;
using CCPDemo.RiskCategory;
using CCPDemo.RiskCategory.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Category)]
    public class CategoryController : CCPDemoControllerBase
    {
        private readonly ICategoryAppService _categoryAppService;

        public CategoryController(ICategoryAppService categoryAppService)
        {
            _categoryAppService = categoryAppService;

        }

        public ActionResult Index()
        {
            var model = new CategoryViewModel();

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Category_Create, AppPermissions.Pages_Administration_Category_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetCategoryForEditOutput getCategoryForEditOutput;

            if (id.HasValue)
            {
                getCategoryForEditOutput = await _categoryAppService.GetCategoryForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getCategoryForEditOutput = new GetCategoryForEditOutput
                {
                    Category = new CreateOrEditCategoryDto()
                };
            }

            var viewModel = new CreateOrEditCategoryModalViewModel()
            {
                Category = getCategoryForEditOutput.Category,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewCategoryModal(int id)
        {
            var getCategoryForViewDto = await _categoryAppService.GetCategoryForView(id);

            var model = new CategoryViewModel()
            {
                Category = getCategoryForViewDto.Category
            };

            return PartialView("_ViewCategoryModal", model);
        }

    }
}