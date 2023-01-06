using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Areas.App.Models.ManagementReviews;
using CCPDemo.Web.Controllers;
using CCPDemo.Authorization;
using CCPDemo.ManagementReviews;
using CCPDemo.ManagementReviews.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_ManagementReviews)]
    public class ManagementReviewsController : CCPDemoControllerBase
    {
        private readonly IManagementReviewsAppService _managementReviewsAppService;

        public ManagementReviewsController(IManagementReviewsAppService managementReviewsAppService)
        {
            _managementReviewsAppService = managementReviewsAppService;

        }

        public ActionResult Index()
        {
            var model = new ManagementReviewsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ManagementReviews_Create, AppPermissions.Pages_Administration_ManagementReviews_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetManagementReviewForEditOutput getManagementReviewForEditOutput;

            if (id.HasValue)
            {
                getManagementReviewForEditOutput = await _managementReviewsAppService.GetManagementReviewForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getManagementReviewForEditOutput = new GetManagementReviewForEditOutput
                {
                    ManagementReview = new CreateOrEditManagementReviewDto()
                };
                getManagementReviewForEditOutput.ManagementReview.submission_date = DateTime.Now;
                getManagementReviewForEditOutput.ManagementReview.next_review = DateTime.Now;
            }

            var viewModel = new CreateOrEditManagementReviewModalViewModel()
            {
                ManagementReview = getManagementReviewForEditOutput.ManagementReview,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewManagementReviewModal(int id)
        {
            var getManagementReviewForViewDto = await _managementReviewsAppService.GetManagementReviewForView(id);

            var model = new ManagementReviewViewModel()
            {
                ManagementReview = getManagementReviewForViewDto.ManagementReview
            };

            return PartialView("_ViewManagementReviewModal", model);
        }

    }
}