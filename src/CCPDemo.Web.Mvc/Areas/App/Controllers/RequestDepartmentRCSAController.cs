using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Abp.Organizations;
using CCPDemo.Authorization;
using CCPDemo.KeyRiskIndicators.Service.Interface;
using CCPDemo.Risks;
using CCPDemo.Web.Areas.App.Models;
using CCPDemo.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_KeyRiskIndicators)]
    public class RequestDepartmentRCSAController : CCPDemoControllerBase
    {
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly RisksAppService _risksAppService;
        private readonly IKRIService _IKRIService;


        public RequestDepartmentRCSAController(IRepository<OrganizationUnit, long> organizationUnitRepository,
                                               RisksAppService risksAppService,
                                               IKRIService IKRIService
                                               )
        {
            _organizationUnitRepository = organizationUnitRepository;
            _risksAppService= risksAppService;
            _IKRIService = IKRIService;
        }
    
        public IActionResult Index()
        {
            var orgToRetrun = _organizationUnitRepository.GetAll();
            RequestDepartmentRCSAModel model = new RequestDepartmentRCSAModel();
            model.OrganizationUnit = orgToRetrun.ToList();
           /* List<OrganizationUnit> result = new List<OrganizationUnit>();
            if (orgToRetrun.Count() > 0)
            {
                foreach (var item in orgToRetrun)
                {
                    if (item.Code.Split(".").Length > 1)
                    {
                        result.Add(item);
                    }
                }
            }
*/
            return View(model);
        }


        public async Task<IActionResult> RequestDepartmentRCSA(int OrganizationUnitId, string message)
        {
            var repsonse = await _risksAppService.UsersInOrganizationalUnit(OrganizationUnitId);
            List<string> emailAddresses = new List<string>();
            if (repsonse.Items.Count > 0)
            {
                foreach (var rep in repsonse.Items)
                {
                    emailAddresses.Add(rep.EmailAddress);
                }
            }
            bool response = await _IKRIService.RequestDepartmentRCSAEmailNotificationAsync(emailAddresses, message);
            return RedirectToAction("Index", "RequestDepartmentRCSA");
        }

    }
}
