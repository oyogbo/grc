using Abp.AspNetCore.Mvc.Authorization;
using CCPDemo.Authorization;
using CCPDemo.Web.Areas.App.Models.Risks;
using CCPDemo.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using CCPDemo.Risks;
using CCPDemo.Risks.Dtos;
using CCPDemo.RiskRatings.Dtos;
using Abp.Domain.Repositories;
using System.Linq.Dynamic.Core;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Risks)]
    public class ReportsController : CCPDemoControllerBase
    {
        private readonly IRisksAppService _risksAppService;
        private readonly IRepository<Risk> _riskRepository;

        public ReportsController(IRisksAppService risksAppService,
            IRepository<Risk> riskRepository)
        {
            _risksAppService = risksAppService;
            _riskRepository = riskRepository;
        }

        public IActionResult Index()
        {
            var model = new RisksViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        public IActionResult OverdueRisks()
        {
            var model = new RisksViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        public IActionResult OnGoingRisks()
        {
            var model = new RisksViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        public IActionResult ClosedRisks()
        {
            var model = new RisksViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        public async Task<IActionResult> FilteredRisks()
        {


            var riskList = await _risksAppService.GetRisks();

            var model = new RisksViewModel
            {
                FilterText = Request.Query["filterText"],
                RiskTypeList = await _risksAppService.GetAllRiskTypeForTableDropdown(),
                RiskOrganizationUnitList = await _risksAppService.GetAllOrganizationUnitForTableDropdown(),
                RiskStatusList = await _risksAppService.GetAllStatusForTableDropdown(),
                RiskRatingList = await _risksAppService.GetAllRiskRatingForTableDropdown(),
                RisksList= riskList
            };


            return View(model);
        }

        public async Task<IActionResult> RiskTypeByDepartment()
        {
			//var riskTypeByDepartment = _risksAppService.GetRiskTypeByDepartment();

			//var model = new RisksViewModel
			//{
			//    RiskTypesByDepartment = riskTypeByDepartment
			//};

			var model = new RisksViewModel
			{
				FilterText = Request.Query["filterText"],
				RiskTypeList = await _risksAppService.GetAllRiskTypeForTableDropdown(),
				RiskOrganizationUnitList = await _risksAppService.GetAllOrganizationUnitForTableDropdown(),
				RiskStatusList = await _risksAppService.GetAllStatusForTableDropdown(),
				RiskRatingList = await _risksAppService.GetAllRiskRatingForTableDropdown(),
			};

			return View(model);
        }

        public async Task<IActionResult> RiskRatingByDepartment()
        {
			var model = new RisksViewModel
			{
				FilterText = Request.Query["filterText"],
				RiskTypeList = await _risksAppService.GetAllRiskTypeForTableDropdown(),
				RiskOrganizationUnitList = await _risksAppService.GetAllOrganizationUnitForTableDropdown(),
				RiskStatusList = await _risksAppService.GetAllStatusForTableDropdown(),
				RiskRatingList = await _risksAppService.GetAllRiskRatingForTableDropdown(),
			};

			return View(model);
        }

    }
}
