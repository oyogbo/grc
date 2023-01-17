using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using CCPDemo.Authorization;
using CCPDemo.Authorization.Roles;
using CCPDemo.KeyRiskIndicatorHistories;
using CCPDemo.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_KeyRiskIndicators)]
    public class KeyRiskIndicatorHistoryController : CCPDemoControllerBase
    {
        private readonly IKeyRiskIndicatorService _keyRiskIndicatorHistoryService;
        private readonly IRepository<Role> _roleRepository;


        public KeyRiskIndicatorHistoryController(IKeyRiskIndicatorService keyRiskIndicatorHistoryService, IRepository<Role> roleRepository)
        {
            _keyRiskIndicatorHistoryService = keyRiskIndicatorHistoryService;
            _roleRepository = roleRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
         var response =  await _keyRiskIndicatorHistoryService.GetAll();
            response = response.OrderByDescending(x => x.Id).ToList();
           var userRoles = await _keyRiskIndicatorHistoryService.GetCurrentUserRoles();

            var rolesfromdb = _roleRepository.GetAll();
            List<string> roles = new List<string>();

            
            foreach (var item in userRoles)
            {
                roles.Add(rolesfromdb.FirstOrDefault(x => x.ConcurrencyStamp == item || x.Name == item).DisplayName);

            }

            if (!roles.Contains("Admin") && !roles.Contains("ERM"))
            {
                long userOrgId = await _keyRiskIndicatorHistoryService.GetUserOrganisationDepartmentId();
                    var filteredList = response.Where(x => x.OrganizationUnit == userOrgId);
                    ViewData["KeyRiskIndicatorHistory"] = filteredList;
                    return View();
            }
            ViewData["KeyRiskIndicatorHistory"] = response;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DeleteKRI(long Id)
        {
            var response = await _keyRiskIndicatorHistoryService.GetAll();
            ViewData["KeyRiskIndicatorHistory"] = response;
            return View();
        }
    }
}
