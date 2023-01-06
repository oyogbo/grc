using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Areas.App.Models.Assessments;
using CCPDemo.Web.Controllers;
using CCPDemo.Authorization;
using CCPDemo.Assessments;
using CCPDemo.Assessments.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Assessments)]
    public class AssessmentsController : CCPDemoControllerBase
    {
        private readonly IAssessmentsAppService _assessmentsAppService;

        public AssessmentsController(IAssessmentsAppService assessmentsAppService)
        {
            _assessmentsAppService = assessmentsAppService;

        }

        public ActionResult Index()
        {
            var model = new AssessmentsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Assessments_Create, AppPermissions.Pages_Assessments_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetAssessmentForEditOutput getAssessmentForEditOutput;

            if (id.HasValue)
            {
                getAssessmentForEditOutput = await _assessmentsAppService.GetAssessmentForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getAssessmentForEditOutput = new GetAssessmentForEditOutput
                {
                    Assessment = new CreateOrEditAssessmentDto()
                };
            }

            var viewModel = new CreateOrEditAssessmentModalViewModel()
            {
                Assessment = getAssessmentForEditOutput.Assessment,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewAssessmentModal(int id)
        {
            var getAssessmentForViewDto = await _assessmentsAppService.GetAssessmentForView(id);

            var model = new AssessmentViewModel()
            {
                Assessment = getAssessmentForViewDto.Assessment
            };

            return PartialView("_ViewAssessmentModal", model);
        }

    }
}