using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Authorization;
using CCPDemo.DashboardCustomization;
using System.Threading.Tasks;
using CCPDemo.Web.Areas.App.Startup;
using Abp.Domain.Repositories;
using System.Linq;
using System.Linq.Dynamic.Core;
using CCPDemo.Risks;
using CCPDemo.Web.Areas.App.Models.Risks;
using Microsoft.EntityFrameworkCore;
using Abp.Organizations;
using CCPDemo.Risks.Dtos;
using System.Collections.Generic;
using CCPDemo.RiskRatings;
using CCPDemo.RiskStatus;
using CCPDemo.RiskTypes;
using CCPDemo.Authorization.Users;
using System;
using Newtonsoft.Json;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Dashboard)]
    public class TenantDashboardController : CustomizableDashboardControllerBase
    {
        private readonly IRepository<Risk> _riskRepository;
        private readonly RisksAppService _risksAppService;
		private readonly IRepository<OrganizationUnit, long> _lookup_organizationUnitRepository;
        private readonly IRepository<RiskType, int> _lookup_riskTypeRepository;
        private readonly IRepository<Status, int> _lookup_statusRepository;
        private readonly IRepository<RiskRating, int> _lookup_riskRatingRepository;
        private readonly IRepository<User, long> _lookup_userRepository;

        public TenantDashboardController(DashboardViewConfiguration dashboardViewConfiguration, 
            IDashboardCustomizationAppService dashboardCustomizationAppService,
            IRepository<Risk> riskRepository, RisksAppService risksAppService,
			IRepository<OrganizationUnit, long> lookup_organizationUnitRepository,
             IRepository<RiskType, int> lookup_riskTypeRepository,
            IRepository<Status, int> lookup_statusRepository,
            IRepository<RiskRating, int> lookup_riskRatingRepository,
            IRepository<User, long> lookup_userRepository, UserManager userManager) 
            : base(dashboardViewConfiguration, dashboardCustomizationAppService)
        {
            _riskRepository= riskRepository;
            _risksAppService=risksAppService;
			_lookup_organizationUnitRepository = lookup_organizationUnitRepository;
            _lookup_riskTypeRepository = lookup_riskTypeRepository;
            _lookup_organizationUnitRepository = lookup_organizationUnitRepository;
            _lookup_statusRepository = lookup_statusRepository;
            _lookup_riskRatingRepository = lookup_riskRatingRepository;
            _lookup_userRepository = lookup_userRepository;
        }

        public async Task<ActionResult> Index()
        {
            //return await GetView(CCPDemoDashboardCustomizationConsts.DashboardNames.DefaultTenantDashboard);

            var filteredRisks = _riskRepository.GetAll()
                .Include(e=>e.OrganizationUnitFk);
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
            var low = _riskRepository.GetAll().Where(r=>r.RiskRatingId == 5).Count();
            var medium = _riskRepository.GetAll().Where(r => r.RiskRatingId == 4).Count();
            var high = _riskRepository.GetAll().Where(r => r.RiskRatingId == 3).Count();
            var veryHigh = _riskRepository.GetAll().Where(r => r.RiskRatingId == 2).Count();
            var critical = _riskRepository.GetAll().Where(r => r.RiskRatingId == 1).Count();

            ViewBag.dbList = dbList;



            var filteredRisks2 = _riskRepository.GetAll()
                .Where(u => u.IsDeleted == false)
                .Include(e => e.RiskTypeFk)
                .Include(e => e.OrganizationUnitFk);

            var query = (from o in filteredRisks2
                         join o1 in _lookup_riskTypeRepository.GetAll() on o.RiskTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new
                         {
                             o.OrganizationUnitId,
                             OrganizationUnit = s2 == null || s2.DisplayName == null ? "" : s2.DisplayName.ToString(),
                             RiskType = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             o.RiskTypeId
                         }).GroupBy(x => new { x.OrganizationUnitId, x.RiskTypeId }).Select(x => new RiskTypeByDepartment
                         {
                             OrganizationUnit = x.First().OrganizationUnit,
                             RiskType = x.First().RiskType,
                             RiskCount = x.Count().ToString(),
                         });

            var riskCountList = query.ToList();

			string riskCountStr = JsonConvert.SerializeObject(riskCountList);



			var filteredRisks0 = _riskRepository.GetAll()
				.Where(u => u.IsDeleted == false)
				.Include(e => e.RiskRatingFk)
				.Include(e => e.OrganizationUnitFk);


			List<RiskRatingByDepartmentDto> riskTypeByDept = new();

			var RatingCountQuery = (from o in filteredRisks0
									join o1 in _lookup_riskRatingRepository.GetAll() on o.RiskRatingId equals o1.Id into j1
									from s1 in j1.DefaultIfEmpty()

									join o2 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o2.Id into j2
									from s2 in j2.DefaultIfEmpty()

									select new
									{
										o.OrganizationUnitId,
										OrganizationUnit = s2 == null || s2.DisplayName == null ? "" : s2.DisplayName.ToString(),
										RiskRating = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
										o.RiskRatingId
									}).GroupBy(x => new { x.OrganizationUnitId, x.RiskRatingId }).Select(x => new RiskRatingByDepartmentDto
									{
										OrganizationUnit = x.First().OrganizationUnit,
										RiskRating = x.First().RiskRating,
										RatingCount = x.Count().ToString(),
									});
            var ratingCountList = RatingCountQuery.ToList();



			var orgUnitForRiskTypeQuery = (from o in filteredRisks2
                          join o1 in _lookup_riskTypeRepository.GetAll() on o.RiskTypeId equals o1.Id into j1
                          from s1 in j1.DefaultIfEmpty()

                          join o2 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o2.Id into j2
                          from s2 in j2.DefaultIfEmpty()

                          select new
                          {
                              OrganizationUnit = s2 == null || s2.DisplayName == null ? "" : s2.DisplayName.ToString()
                          }).Distinct().ToArray();
            List<string> orgUnitList1 = new();
            foreach(var ou in orgUnitForRiskTypeQuery)
            {
                orgUnitList1.Add(ou.OrganizationUnit.ToString());
            }


			var RiskTypesQuery = (from o in filteredRisks2
										   join o1 in _lookup_riskTypeRepository.GetAll() on o.RiskTypeId equals o1.Id into j1
										   from s1 in j1.DefaultIfEmpty()

										   join o2 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o2.Id into j2
										   from s2 in j2.DefaultIfEmpty()

										   select new
										   {
											   RiskType = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
										   }).Distinct().ToArray();

            var RiskRatingQuery = (from o in filteredRisks2
										   join o1 in _lookup_riskRatingRepository.GetAll() on o.RiskRatingId equals o1.Id into j1
										   from s1 in j1.DefaultIfEmpty()

										   join o2 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o2.Id into j2
										   from s2 in j2.DefaultIfEmpty()

										   select new
										   {
											   RiskRating = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
										   }).Distinct().ToArray();


            List<string> riskTypesList = new();
            foreach (var r in RiskTypesQuery)
            {
                riskTypesList.Add(r.RiskType.ToString());
            }

            List<string> riskRatingList = new();
            foreach (var r in RiskRatingQuery)
            {
                riskRatingList.Add(r.RiskRating.ToString());
            }




            var riskQuery = (from o in filteredRisks2
                             join o1 in _lookup_riskTypeRepository.GetAll() on o.RiskTypeId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             join o2 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o2.Id into j2
                             from s2 in j2.DefaultIfEmpty()

                             select new RiskTypeBarchartInitDto
                             {
                                 OrgUnitId = o.OrganizationUnitId,
                                 OrgUnitName = s2 == null || s2.DisplayName == null ? "" : s2.DisplayName.ToString(),
                                 RiskType = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             }).ToList();


            var riskTypeByOrUnit = query.ToList();

            foreach (var rt in riskTypesList)
            {
				foreach (var ou in orgUnitList1)
				{

				}
			}

            
			//var distinctOrgUnitsList = (from o in query select new { o.OrganizationUnit }).Distinct().ToList();
			//         var distinctRiskTypeList = (from o in query select new { o.RiskType }).Distinct().ToList();

			//         List<string> orgUnitLabels = new();
			//         List<string> risksTypeLabel = new();
			//         List<string[]> riskDataList = new();

			//         foreach (var ou in distinctOrgUnitsList)
			//         {
			//             orgUnitLabels.Add(ou.OrganizationUnit);
			//         }

			//         foreach (var riskTpe in distinctRiskTypeList)
			//         {
			//             risksTypeLabel.Add(riskTpe.RiskType);
			//             string[] riskCounts = (from o in query where o.RiskType.ToString() == riskTpe.ToString() select o.RiskCount ?? "0").ToArray();
			//             riskDataList.Add(riskCounts);
			//         }


			var model = new RiskViewModel
            {
                RiskCount = riskCount,
                Risks = risks,
                Low = low,
                Medium = medium,
                Critical = critical,
                High = high,
                VeryHigh= veryHigh,
                dbList = tableData,
				//OrganizationUnitLabels = orgUnitLabels,
				//RiskTypeLabels = risksTypeLabel,
				//RiskTypeCountCountData = riskDataList
				RiskTypeOrgUnitsUnits = orgUnitList1,
                RiskTypes = riskTypesList,
                RiskRating = riskRatingList,
                //RiskCountByDepartment = riskCountList,
                RiskCountByDepartment = riskCountStr,
                RatingCountByDepartment = ratingCountList

			};

            return View(model);
        }

        public void Utility()
        {
            //var query = _riskRepository.GetAll()
            //            .Include(e => e.RiskTypeFk)
            //            .Include(e => e.OrganizationUnitFk)
            //            .Include(e => e.StatusFk)
            //            .Include(e => e.RiskRatingFk)
            //            .Include(e => e.UserFk);


            //var risks = from o in query
            //            join o1 in _lookup_riskTypeRepository.GetAll() on o.RiskTypeId equals o1.Id into j1
            //            from s1 in j1.DefaultIfEmpty()

            //            join o2 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o2.Id into j2
            //            from s2 in j2.DefaultIfEmpty()

            //            join o3 in _lookup_statusRepository.GetAll() on o.StatusId equals o3.Id into j3
            //            from s3 in j3.DefaultIfEmpty()

            //            join o4 in _lookup_riskRatingRepository.GetAll() on o.RiskRatingId equals o4.Id into j4
            //            from s4 in j4.DefaultIfEmpty()

            //            join o5 in _lookup_userRepository.GetAll() on o.UserId equals o5.Id into j5
            //            from s5 in j5.DefaultIfEmpty()

            //            select new
            //            {

            //                o.Summary,
            //                o.ExistingControl,
            //                o.ERMRecommendation,
            //                o.ActionPlan,
            //                o.RiskOwnerComment,
            //                o.TargetDate,
            //                o.ActualClosureDate,
            //                o.AcceptanceDate,
            //                o.RiskAccepted,
            //                Id = o.Id,
            //                o.UserId,
            //                RiskTypeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
            //                OrganizationUnitDisplayName = s2 == null || s2.DisplayName == null ? "" : s2.DisplayName.ToString(),
            //                StatusName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
            //                RiskRatingName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
            //                UserName = s5 == null || s5.Name == null ? "" : s5.Name.ToString()
            //            };

            //var orgUnits = (from o in risks select new {o.OrganizationUnitDisplayName}).Distinct().ToList();

            //foreach(var ou in orgUnits)
            //{

            //}

            var filteredRisks2 = _riskRepository.GetAll()
                .Where(u => u.IsDeleted == false)
                .Include(e => e.RiskTypeFk)
                .Include(e => e.OrganizationUnitFk);

            var query = (from o in filteredRisks2
                         join o1 in _lookup_riskTypeRepository.GetAll() on o.RiskTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new
                         {
                             o.OrganizationUnitId,
                             OrganizationUnit = s2 == null || s2.DisplayName == null ? "" : s2.DisplayName.ToString(),
                             RiskType = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             o.RiskTypeId
                         }).GroupBy(x => new { x.OrganizationUnitId, x.RiskTypeId }).Select(x => new RiskTypeByDepartment
                         {
                             OrganizationUnit = x.First().OrganizationUnit,
                             RiskType = x.First().RiskType,
                             RiskCount = x.Count().ToString(),
                         });



			var filteredRisks0 = _riskRepository.GetAll()
				.Where(u => u.IsDeleted == false)
				.Include(e => e.RiskRatingFk)
				.Include(e => e.OrganizationUnitFk);


			List<RiskRatingByDepartmentDto> riskTypeByDept = new();

			var RatingCountQuery = (from o in filteredRisks0
						 join o1 in _lookup_riskRatingRepository.GetAll() on o.RiskRatingId equals o1.Id into j1
						 from s1 in j1.DefaultIfEmpty()

						 join o2 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o2.Id into j2
						 from s2 in j2.DefaultIfEmpty()

						 select new
						 {
							 o.OrganizationUnitId,
							 OrganizationUnit = s2 == null || s2.DisplayName == null ? "" : s2.DisplayName.ToString(),
							 RiskRating = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
							 o.RiskRatingId
						 }).GroupBy(x => new { x.OrganizationUnitId, x.RiskRatingId }).Select(x => new RiskRatingByDepartmentDto
						 {
							 OrganizationUnit = x.First().OrganizationUnit,
							 RiskRating = x.First().RiskRating,
							 RatingCount = x.Count().ToString(),
						 });


			var distinctOrgUnitsList = (from o in query select new {o.OrganizationUnit}).Distinct().ToList();
            var distinctRiskTypeList = (from o in query select new {o.RiskType}).Distinct().ToList();

            List<string> orgUnitLabels = new();
            List<string> risksTypeLabel = new();
            List<string[]> dataList = new();

            foreach (var ou in distinctOrgUnitsList)
            {
                orgUnitLabels.Add(ou.OrganizationUnit);               
            }

            foreach (var riskTpe in distinctRiskTypeList)
            {
                risksTypeLabel.Add(riskTpe.RiskType);
                string[] riskCounts = (from o in query where o.RiskType.ToString() == riskTpe.ToString() select o.RiskCount??"0").ToArray();
                dataList.Add(riskCounts);
            }
        }
    }
}