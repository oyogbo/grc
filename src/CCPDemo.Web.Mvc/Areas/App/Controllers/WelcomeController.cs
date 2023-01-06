using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Controllers;
using Abp.Domain.Repositories;
using CCPDemo.VRisks;
using CCPDemo.Web.Areas.App.Models.VRisks;
using Stripe;
using System.Linq;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize]
    public class WelcomeController : CCPDemoControllerBase
    {
		private readonly IRepository<VRisk> _vRiskRepository;

        public WelcomeController(IRepository<VRisk> vRiskRepository)
        {
            _vRiskRepository = vRiskRepository;
        }
		public ActionResult Index()
        {
			var risks = _vRiskRepository.GetAllList();
			var low = _vRiskRepository.GetAll().Where(rating => rating.Rating.ToUpper() == "LOW").Count();
			var medium = _vRiskRepository.GetAll().Where(rating => rating.Rating.ToUpper() == "MEDIUM").Count();
			var high = _vRiskRepository.GetAll().Where(rating => rating.Rating.ToUpper() == "HIGH").Count();
			var critical = _vRiskRepository.GetAll().Where(rating => rating.Rating.ToUpper() == "CRITICAL").Count();
			var model = new VRisksViewModel
			{
				Risks = risks,
				Low = low,
				Medium = medium,
				Critical = critical,
				High = high,

			};
			return View(model);
        }
    }
}