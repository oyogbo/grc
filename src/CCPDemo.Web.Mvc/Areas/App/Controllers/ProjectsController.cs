using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Areas.App.Models.Projects;
using CCPDemo.Web.Controllers;
using CCPDemo.Authorization;
using CCPDemo.Projects;
using CCPDemo.Projects.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Projects)]
    public class ProjectsController : CCPDemoControllerBase
    {
        private readonly IProjectsAppService _projectsAppService;

        public ProjectsController(IProjectsAppService projectsAppService)
        {
            _projectsAppService = projectsAppService;

        }

        public ActionResult Index()
        {
            var model = new ProjectsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Projects_Create, AppPermissions.Pages_Administration_Projects_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetProjectForEditOutput getProjectForEditOutput;

            if (id.HasValue)
            {
                getProjectForEditOutput = await _projectsAppService.GetProjectForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getProjectForEditOutput = new GetProjectForEditOutput
                {
                    Project = new CreateOrEditProjectDto()
                };
                getProjectForEditOutput.Project.DueDate = DateTime.Now;
            }

            var viewModel = new CreateOrEditProjectModalViewModel()
            {
                Project = getProjectForEditOutput.Project,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewProjectModal(int id)
        {
            var getProjectForViewDto = await _projectsAppService.GetProjectForView(id);

            var model = new ProjectViewModel()
            {
                Project = getProjectForViewDto.Project
            };

            return PartialView("_ViewProjectModal", model);
        }

    }
}