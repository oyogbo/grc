using Abp.AspNetCore.Mvc.Authorization;
using Abp.UI;
using Abp.Web.Models;
using CCPDemo.Authorization;
using CCPDemo.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_KeyRiskIndicators)]
    public class UploadKeyRiskIndicatorController : CCPDemoControllerBase
    {
        public IActionResult Index()
        {
            return View();
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
    }
}
