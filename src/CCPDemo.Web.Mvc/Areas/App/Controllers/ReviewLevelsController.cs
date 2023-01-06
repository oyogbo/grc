using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Areas.App.Models.ReviewLevels;
using CCPDemo.Web.Controllers;
using CCPDemo.Authorization;
using CCPDemo.ReviewLevels;
using CCPDemo.ReviewLevels.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_ReviewLevels)]
    public class ReviewLevelsController : CCPDemoControllerBase
    {
        private readonly IReviewLevelsAppService _reviewLevelsAppService;

        public ReviewLevelsController(IReviewLevelsAppService reviewLevelsAppService)
        {
            _reviewLevelsAppService = reviewLevelsAppService;

        }

        public ActionResult Index()
        {
            var model = new ReviewLevelsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ReviewLevels_Create, AppPermissions.Pages_Administration_ReviewLevels_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetReviewLevelForEditOutput getReviewLevelForEditOutput;

            if (id.HasValue)
            {
                getReviewLevelForEditOutput = await _reviewLevelsAppService.GetReviewLevelForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getReviewLevelForEditOutput = new GetReviewLevelForEditOutput
                {
                    ReviewLevel = new CreateOrEditReviewLevelDto()
                };
            }

            var viewModel = new CreateOrEditReviewLevelModalViewModel()
            {
                ReviewLevel = getReviewLevelForEditOutput.ReviewLevel,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewReviewLevelModal(int id)
        {
            var getReviewLevelForViewDto = await _reviewLevelsAppService.GetReviewLevelForView(id);

            var model = new ReviewLevelViewModel()
            {
                ReviewLevel = getReviewLevelForViewDto.ReviewLevel
            };

            return PartialView("_ViewReviewLevelModal", model);
        }

    }
}