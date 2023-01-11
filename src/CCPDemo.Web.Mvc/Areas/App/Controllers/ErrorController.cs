using CCPDemo.Web.Areas.App.Models.Error;
using CCPDemo.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault.Models;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    public class ErrorController : CCPDemoControllerBase
    {
        public IActionResult Index(ErrorView errorView)
        {
            return View(errorView);
        }
    }
}
