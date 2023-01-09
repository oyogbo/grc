using Abp.AspNetCore.Mvc.Authorization;
using CCPDemo.Authorization;
using CCPDemo.KeyRiskIndicatorHistories;
using CCPDemo.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_KeyRiskIndicators)]
    public class KeyRiskIndicatorHistoryController : CCPDemoControllerBase
    {
        private readonly IKeyRiskIndicatorService _keyRiskIndicatorHistoryService;

        public KeyRiskIndicatorHistoryController(IKeyRiskIndicatorService keyRiskIndicatorHistoryService)
        {
            _keyRiskIndicatorHistoryService = keyRiskIndicatorHistoryService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
         var response =  await _keyRiskIndicatorHistoryService.GetAll();
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
