using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Areas.App.Models.UsersLookups;
using CCPDemo.Web.Controllers;
using CCPDemo.Authorization;
using CCPDemo.UsersLookups;
using CCPDemo.UsersLookups.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_UsersLookups)]
    public class UsersLookupsController : CCPDemoControllerBase
    {
        private readonly IUsersLookupsAppService _usersLookupsAppService;

        public UsersLookupsController(IUsersLookupsAppService usersLookupsAppService)
        {
            _usersLookupsAppService = usersLookupsAppService;

        }

        public ActionResult Index()
        {
            var model = new UsersLookupsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_UsersLookups_Create, AppPermissions.Pages_UsersLookups_Edit)]
        public async Task<ActionResult> CreateOrEdit(int? id)
        {
            GetUsersLookupForEditOutput getUsersLookupForEditOutput;

            if (id.HasValue)
            {
                getUsersLookupForEditOutput = await _usersLookupsAppService.GetUsersLookupForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getUsersLookupForEditOutput = new GetUsersLookupForEditOutput
                {
                    UsersLookup = new CreateOrEditUsersLookupDto()
                };
            }

            var viewModel = new CreateOrEditUsersLookupViewModel()
            {
                UsersLookup = getUsersLookupForEditOutput.UsersLookup,
                UserName = getUsersLookupForEditOutput.UserName,
            };

            return View(viewModel);
        }

        public async Task<ActionResult> ViewUsersLookup(int id)
        {
            var getUsersLookupForViewDto = await _usersLookupsAppService.GetUsersLookupForView(id);

            var model = new UsersLookupViewModel()
            {
                UsersLookup = getUsersLookupForViewDto.UsersLookup
                ,
                UserName = getUsersLookupForViewDto.UserName

            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_UsersLookups_Create, AppPermissions.Pages_UsersLookups_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new UsersLookupUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_UsersLookupUserLookupTableModal", viewModel);
        }

    }
}