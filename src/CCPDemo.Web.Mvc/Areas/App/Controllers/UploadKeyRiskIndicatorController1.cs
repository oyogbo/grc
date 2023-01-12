using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Abp.Organizations;
using Abp.UI;
using Abp.Web.Models;
using CCPDemo.Authorization;
using CCPDemo.Web.Areas.App.Models.UploadKeyRiskIndicators;
using CCPDemo.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_KeyRiskIndicators)]
    public class UploadKeyRiskIndicatorController : CCPDemoControllerBase
    {
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;

        public UploadKeyRiskIndicatorController(IRepository<OrganizationUnit, long> organizationUnitRepository)
        {
            _organizationUnitRepository = organizationUnitRepository;
        }

        public IActionResult Index()
        {
            var orgToRetrun = _organizationUnitRepository.GetAll();
            List<OrganizationUnit> result = new List<OrganizationUnit>();
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

            UploadKeyRiskIndicator model = new UploadKeyRiskIndicator();
            model.OrganizationUnits = result;
            return View(model);
        }

        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Tenant_Settings)]
        public async Task<JsonResult> UploadKRI()
        {
            try
            {
                return Json(new AjaxResponse(new
                {


                }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }

        }

       /* [HttpGet]
        public async Task<IActionResult>GetAllOrganisationUnit( )
        {
           var orgToRetrun = _organizationUnitRepository.GetAll();
            List<OrganizationUnit> result = new List<OrganizationUnit>();   
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

            var settings = new Newtonsoft.Json.JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            return Json(result, settings);

        }*/

    }
}
