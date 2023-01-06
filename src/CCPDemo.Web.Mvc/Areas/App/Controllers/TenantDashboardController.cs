using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Authorization;
using CCPDemo.DashboardCustomization;
using System.Threading.Tasks;
using CCPDemo.Web.Areas.App.Startup;
using Abp.Domain.Repositories;
using CCPDemo.VRisks;
using CCPDemo.Web.Areas.App.Models.VRisks;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Dashboard)]
    public class TenantDashboardController : CustomizableDashboardControllerBase
    {
        private readonly IRepository<VRisk> _vRiskRepository;
        private readonly VRisksAppService _vriskAppService;
   
        public TenantDashboardController(DashboardViewConfiguration dashboardViewConfiguration, 
            IDashboardCustomizationAppService dashboardCustomizationAppService, IRepository<VRisk> vRiskRepository,
            VRisksAppService vRisksAppService) 
            : base(dashboardViewConfiguration, dashboardCustomizationAppService)
        {
            _vRiskRepository= vRiskRepository;
            _vriskAppService = vRisksAppService;
        }

        public async Task<ActionResult> Index()
        {
            //return await GetView(CCPDemoDashboardCustomizationConsts.DashboardNames.DefaultTenantDashboard);

            string[] depts = { "OPERATIONS", "SALES", "APPLICATION SUPPORT", "DATA & DIGITAL SERVICES" };
            var opsCount = _vRiskRepository.GetAll().Where(dept => dept.Department.ToUpper() == depts[0]).Count();
            var salesCount = _vRiskRepository.GetAll().Where(dept => dept.Department.ToUpper() == depts[1]).Count();
            var appsCount = _vRiskRepository.GetAll().Where(dept => dept.Department.ToUpper() == depts[2]).Count();
            var digisCount = _vRiskRepository.GetAll().Where(dept => dept.Department.ToUpper() == depts[3]).Count();
            int[] deptsCount = { opsCount, salesCount, appsCount, digisCount };

			var riskCount = _vRiskRepository.Count();
            var risks = _vRiskRepository.GetAllList();
            var low = _vRiskRepository.GetAll().Where(rating => rating.Rating.ToUpper() == "LOW").Count();
            var medium = _vRiskRepository.GetAll().Where(rating => rating.Rating.ToUpper() == "MEDIUM").Count();
            var high = _vRiskRepository.GetAll().Where(rating => rating.Rating.ToUpper() == "HIGH").Count();
            var critical = _vRiskRepository.GetAll().Where(rating => rating.Rating.ToUpper() == "CRITICAL").Count();
            var model = new VRisksViewModel
            {
                RiskCount = riskCount,
                Risks = risks,
                Low = low,
                Medium = medium,
                Critical = critical,
                High = high,
                DepartmentsCounts = deptsCount,
                
            };

            return View(model);
        }
    }
}