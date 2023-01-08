using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Controllers;
using Abp.Domain.Repositories;
using Stripe;
using System.Linq;
using Abp.Organizations;
using CCPDemo.Risks;
using CCPDemo.Risks.Dtos;
using CCPDemo.Web.Areas.App.Models.Risks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize]
    public class WelcomeController : CCPDemoControllerBase
    {
		private readonly IRepository<Risk> _riskRepository;
		private readonly RisksAppService _risksAppService;
		private readonly IRepository<OrganizationUnit, long> _lookup_organizationUnitRepository;

		public WelcomeController(IRepository<Risk> riskRepository, RisksAppService risksAppService,
			IRepository<OrganizationUnit, long> lookup_organizationUnitRepository)
        {
			_riskRepository = riskRepository;
			_risksAppService = risksAppService;
			_lookup_organizationUnitRepository = lookup_organizationUnitRepository;
		}
		public async Task<ActionResult> Index()
        {

			var filteredRisks = _riskRepository.GetAll()
				.Include(e => e.OrganizationUnitFk);
			var risksTable = from o in filteredRisks

							 join o1 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o1.Id into j1
							 from s1 in j1.DefaultIfEmpty()
							 select new
							 {
								 o.Summary,
								 OrganizationUnitDisplayName = s1 == null || s1.DisplayName == null ? "" : s1.DisplayName.ToString(),
								 o.TargetDate
							 };

			var dbList = await risksTable.ToListAsync();


			var tableData = new List<RiskTableForDisplayDto>();
			foreach (var data in dbList)
			{
				var riskTable = new RiskTableForDisplayDto
				{
					Summary = data.Summary,
					OrganizationUnitDisplayName = data.OrganizationUnitDisplayName,
					TargetDate = data.TargetDate,
				};
				tableData.Add(riskTable);
			}


			var riskCount = _riskRepository.Count();
			var risks = _riskRepository.GetAllList();
			var low = _riskRepository.GetAll().Where(r => r.RiskRatingId == 5).Count();
			var medium = _riskRepository.GetAll().Where(r => r.RiskRatingId == 4).Count();
			var high = _riskRepository.GetAll().Where(r => r.RiskRatingId == 3).Count();
			var veryHigh = _riskRepository.GetAll().Where(r => r.RiskRatingId == 2).Count();
			var critical = _riskRepository.GetAll().Where(r => r.RiskRatingId == 1).Count();

			ViewBag.dbList = dbList;

			var model = new RiskViewModel
			{
				RiskCount = riskCount,
				Risks = risks,
				Low = low,
				Medium = medium,
				Critical = critical,
				High = high,
				VeryHigh = veryHigh,
				dbList = tableData,
			};


			return View(model);
        }
    }
}