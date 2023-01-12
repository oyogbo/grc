using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Areas.App.Models.Risks;
using CCPDemo.Web.Controllers;
using CCPDemo.Authorization;
using CCPDemo.Risks;
using CCPDemo.Risks.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using CCPDemo.RiskTransactions.Dtos;
using CCPDemo.RiskTransactions;
using Abp.Domain.Repositories;
using CCPDemo.Authorization.Users;
using System.Linq;
using Abp.Zero.Configuration;
using CCPDemo.Authorization.Roles;
using CCPDemo.Authorization.Roles.Dto;
using System.Collections.Generic;
using Abp.Authorization.Users;
using Abp.Collections.Extensions;
using AutoMapper;
using CCPDemo.Dto;
using CCPDemo.EntityFrameworkCore;
using Abp.EntityFrameworkCore;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Risks)]
    public class RisksController : CCPDemoControllerBase
    {
        private readonly IRisksAppService _risksAppService;
        private readonly IRepository<RiskTransaction> _riskTransactionRepository;
        private readonly IRepository<Risk> _riskRepository;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRoleManagementConfig _roleManagementConfig;
        private readonly IRoleAppService _roleAppService;
        private readonly IRepository<Role> _roleRepository;
        //private readonly CCPDemoDbContext _ctx;


        public RisksController(IRisksAppService risksAppService, 
            IRepository<Risk> riskRepository, 
            IRepository<RiskTransaction> riskTransactionRepository,
            UserManager userManager,
            RoleManager roleManager,
            IRoleManagementConfig roleManagementConfig,
            IRoleAppService roleAppService,
            IRepository<Role> roleRepository
            /*IDbContextProvider<CCPDemoDbContext> dbContextProvider*/)
        {
            _risksAppService = risksAppService;
            _riskRepository = riskRepository;
            _riskTransactionRepository = riskTransactionRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _roleManagementConfig = roleManagementConfig;
            _roleAppService = roleAppService;
            _roleRepository = roleRepository;
            //_ctx = dbContextProvider.GetDbContext();
        }

        public ActionResult Index()
        {
            var model = new RisksViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Risks_Create, AppPermissions.Pages_Risks_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            
            var user = _userManager.Users.Where(u=>u.Id == AbpSession.UserId).FirstOrDefault();

            var users = _userManager.Users.Where(u => u.IsDeleted == false).ToList();
            //var query = _userManager.Users.AsNoTracking();
            //query = from u in query where u.Id == AbpSession.UserId select u;
            //IEnumerable<UserRole> rr = from r in query.FirstOrDefault().Roles select r;

            var rolesDump = _roleRepository.GetAll().Where(r => r.IsDeleted == false);

            var roleData = from r in rolesDump
                           select new
                           {
                               r.Id,
                               r.Name,
                               r.DisplayName
                           };

            Dictionary<int, string> roleValues = new Dictionary<int, string>();
            foreach (var role in roleData)
            {
                roleValues.Add(role.Id, role.Name + "=" + role.DisplayName);
            }

            var usersQuery = from u in users
                             select new
                             {
                                 u.EmailAddress,
                                 u.Id
                             };

            var UsersEmailsIdsDict = new Dictionary<string, int>();
            foreach (var ueIds in usersQuery)
            {
                UsersEmailsIdsDict.Add(ueIds.EmailAddress, (int)ueIds.Id);
            }




            var isAdmin = _userManager.IsInRoleAsync(user, "Admin").Result;
            var isUser = _userManager.IsInRoleAsync(user, "User").Result ? "User" : "Admin";
            

            var userRolesList = _userManager.GetRolesAsync(user).Result;


            var userRolesNames = new List<string>();
            foreach (var role in roleData)
            {
                if (userRolesList.Contains(role.Name))
                {
                    userRolesNames.Add(role.DisplayName.ToUpper());
                }
            }

            var isERM = userRolesNames.Contains("ERM");

            




            GetRiskForEditOutput getRiskForEditOutput;

            if (id.HasValue)
            {
                getRiskForEditOutput = await _risksAppService.GetRiskForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getRiskForEditOutput = new GetRiskForEditOutput
                {
                    Risk = new CreateOrEditRiskDto()
                };
                getRiskForEditOutput.Risk.TargetDate = DateTime.Now;
                getRiskForEditOutput.Risk.ActualClosureDate = DateTime.Now;
                getRiskForEditOutput.Risk.AcceptanceDate = DateTime.Now;
            }

            var staticRoleNames = _roleManagementConfig.StaticRoles.Select(r => r.RoleName).ToList();
           
            var viewModel = new CreateOrEditRiskModalViewModel()
            {
                Risk = getRiskForEditOutput.Risk,
                RiskTypeName = getRiskForEditOutput.RiskTypeName,
                OrganizationUnitDisplayName = getRiskForEditOutput.OrganizationUnitDisplayName,
                StatusName = getRiskForEditOutput.StatusName,
                RiskRatingName = getRiskForEditOutput.RiskRatingName,
                UserName = getRiskForEditOutput.UserName,
                RiskRiskTypeList = await _risksAppService.GetAllRiskTypeForTableDropdown(),
                RiskOrganizationUnitList = await _risksAppService.GetAllOrganizationUnitForTableDropdown(),
                RiskStatusList = await _risksAppService.GetAllStatusForTableDropdown(),
                RiskRiskRatingList = await _risksAppService.GetAllRiskRatingForTableDropdown(),
                RiskUserList = await _risksAppService.GetAllUserForTableDropdown(),
                isAdmin = isAdmin,
                isERM = isERM,
                userText = isUser,
                //ERMText = isERMText,
                //RoleNames = myRoles,
                RoleList = roleValues,
                //UserRolesInfo = userRoleValues,
                UserRoles = userRolesList,
                UsersEmailsIdsDict = UsersEmailsIdsDict
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Risks_Create)]
        public async Task<PartialViewResult> ErmTransferRisk(int? id)
        {
            GetRiskForEditOutput getRiskForEditOutput;

            if (id.HasValue)
            {
                getRiskForEditOutput = await _risksAppService.GetRiskForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getRiskForEditOutput = new GetRiskForEditOutput
                {
                    Risk = new CreateOrEditRiskDto()
                };
                getRiskForEditOutput.Risk.TargetDate = DateTime.Now;
                getRiskForEditOutput.Risk.ActualClosureDate = DateTime.Now;
                getRiskForEditOutput.Risk.AcceptanceDate = DateTime.Now;
            }

            var viewModel = new CreateOrEditRiskModalViewModel()
            {
                Risk = getRiskForEditOutput.Risk,
                RiskTypeName = getRiskForEditOutput.RiskTypeName,
                OrganizationUnitDisplayName = getRiskForEditOutput.OrganizationUnitDisplayName,
                StatusName = getRiskForEditOutput.StatusName,
                RiskRatingName = getRiskForEditOutput.RiskRatingName,
                UserName = getRiskForEditOutput.UserName,
                RiskRiskTypeList = await _risksAppService.GetAllRiskTypeForTableDropdown(),
                RiskOrganizationUnitList = await _risksAppService.GetAllOrganizationUnitForTableDropdown(),
                RiskStatusList = await _risksAppService.GetAllStatusForTableDropdown(),
                RiskRiskRatingList = await _risksAppService.GetAllRiskRatingForTableDropdown(),
                RiskUserList = await _risksAppService.GetAllUserForTableDropdown(),

            };

            return PartialView("_ErmTransferRisk", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Risks_Create)]
        public async Task<PartialViewResult> ErmUpgradeOrDowngradeRisk(int? id)
        {
            GetRiskForEditOutput getRiskForEditOutput;

            if (id.HasValue)
            {
                getRiskForEditOutput = await _risksAppService.GetRiskForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getRiskForEditOutput = new GetRiskForEditOutput
                {
                    Risk = new CreateOrEditRiskDto()
                };
                getRiskForEditOutput.Risk.TargetDate = DateTime.Now;
                getRiskForEditOutput.Risk.ActualClosureDate = DateTime.Now;
                getRiskForEditOutput.Risk.AcceptanceDate = DateTime.Now;
            }

            var viewModel = new CreateOrEditRiskModalViewModel()
            {
                Risk = getRiskForEditOutput.Risk,
                RiskTypeName = getRiskForEditOutput.RiskTypeName,
                OrganizationUnitDisplayName = getRiskForEditOutput.OrganizationUnitDisplayName,
                StatusName = getRiskForEditOutput.StatusName,
                RiskRatingName = getRiskForEditOutput.RiskRatingName,
                UserName = getRiskForEditOutput.UserName,
                RiskRiskTypeList = await _risksAppService.GetAllRiskTypeForTableDropdown(),
                RiskOrganizationUnitList = await _risksAppService.GetAllOrganizationUnitForTableDropdown(),
                RiskStatusList = await _risksAppService.GetAllStatusForTableDropdown(),
                RiskRiskRatingList = await _risksAppService.GetAllRiskRatingForTableDropdown(),
                RiskUserList = await _risksAppService.GetAllUserForTableDropdown(),
                isERM = IsERM()

            };

            return PartialView("_ErmUpgradeOrDowngradeRisk", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Risks_Create)]
        public async Task<PartialViewResult> ErmCloseRisk(int? id)
        {
            GetRiskForEditOutput getRiskForEditOutput;

            if (id.HasValue)
            {
                getRiskForEditOutput = await _risksAppService.GetRiskForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getRiskForEditOutput = new GetRiskForEditOutput
                {
                    Risk = new CreateOrEditRiskDto()
                };
                getRiskForEditOutput.Risk.TargetDate = DateTime.Now;
                getRiskForEditOutput.Risk.ActualClosureDate = DateTime.Now;
                getRiskForEditOutput.Risk.AcceptanceDate = DateTime.Now;
            }

            var viewModel = new CreateOrEditRiskModalViewModel()
            {
                Risk = getRiskForEditOutput.Risk,
                RiskTypeName = getRiskForEditOutput.RiskTypeName,
                OrganizationUnitDisplayName = getRiskForEditOutput.OrganizationUnitDisplayName,
                StatusName = getRiskForEditOutput.StatusName,
                RiskRatingName = getRiskForEditOutput.RiskRatingName,
                UserName = getRiskForEditOutput.UserName,
                RiskRiskTypeList = await _risksAppService.GetAllRiskTypeForTableDropdown(),
                RiskOrganizationUnitList = await _risksAppService.GetAllOrganizationUnitForTableDropdown(),
                RiskStatusList = await _risksAppService.GetAllStatusForTableDropdown(),
                RiskRiskRatingList = await _risksAppService.GetAllRiskRatingForTableDropdown(),
                RiskUserList = await _risksAppService.GetAllUserForTableDropdown(),
                isERM = IsERM()

            };

            return PartialView("_ErmCloseRisk", viewModel);
        }

        public async Task<PartialViewResult> ViewRiskModal(int id)
        {
            var user = _userManager.Users.Where(u => u.Id == AbpSession.UserId).FirstOrDefault();

            var users = _userManager.Users.Where(u => u.IsDeleted == false).ToList();

            var rolesDump = _roleRepository.GetAll().Where(r => r.IsDeleted == false);

            var roleData = from r in rolesDump
                           select new
                           {
                               r.Id,
                               r.Name,
                               r.DisplayName
                           };


            var userRolesList = _userManager.GetRolesAsync(user).Result;


            var userRolesNames = new List<string>();
            foreach (var role in roleData)
            {
                if (userRolesList.Contains(role.Name))
                {
                    userRolesNames.Add(role.DisplayName.ToUpper());
                }
            }

            var isERM = userRolesNames.Contains("ERM");


            var getRiskForViewDto = await _risksAppService.GetRiskForView(id);

            var model = new RiskViewModel()
            {
                Risk = getRiskForViewDto.Risk
                ,
                RiskTypeName = getRiskForViewDto.RiskTypeName

                ,
                OrganizationUnitDisplayName = getRiskForViewDto.OrganizationUnitDisplayName

                ,
                StatusName = getRiskForViewDto.StatusName

                ,
                RiskRatingName = getRiskForViewDto.RiskRatingName

                ,
                UserName = getRiskForViewDto.UserName,
                isERM = isERM

            };

            return PartialView("_ViewRiskModal", model);
        }


        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Transfer([FromBody] RiskTransactionM model)
        {

            var riskTransactionDto = new RiskTransaction
            {
                RiskId = model.RiskId,
                CurrentValue = _risksAppService.GetOrganizationalUnitName(model.CurrentValue),
                NewValue = _risksAppService.GetOrganizationalUnitName(model.NewValue),
                UserId = model.UserId,
                Date = model.Date,
                TransactionType = model.TransactionType
            };

            await _riskTransactionRepository.InsertAsync(riskTransactionDto);


            //var riskDto = new CreateOrEditRiskDto
            //{
            //    Id = model.riskId,
            //    Summary = model.summary,
            //    ExistingControl = model.existingControl,
            //    ERMRecommendation = model.ermRecommendation,
            //    ActionPlan = model.actionPlan,
            //    RiskOwnerComment = model.riskOwnerComment,
            //    TargetDate = model.targetDate,
            //    ActualClosureDate = model.actualClosureDate,
            //    AcceptanceDate = model.acceptanceDate,
            //    RiskAccepted = model.riskAccepted,
            //    RiskTypeId = model.riskTypeId,
            //    OrganizationUnitId = Convert.ToInt64(model.organizationUnitId),
            //    StatusId = model.statusId,
            //    RiskRatingId = model.riskRatingId,
            //    UserId = Convert.ToInt64(model.userId),
            //};

            //await _risksAppService.CreateOrEdit(riskDto);

            // Update risk owner
            //var riskUpdate = await _riskRepo.UpdateAsync(riskDto);
            //await _vRisksAppService.CreateOrEdit(upDateRiskDto);


            return Json("Success");

            //return View();
        }

        [Serializable]
        public class RolesModel
        {

            public string Value { get; set; }
            public string DisplayText { get; set; }
            public bool IsSelected { get; set; }

            public RolesModel()
            {
            }

            public RolesModel(string value, string displayText)
            {
                Value = value;
                DisplayText = displayText;
            }
        }

        public class NewRoles
        {
            public string Name { get; set; }
        }

        private IEnumerable<Role> GetRolesOfUsers(List<User> users)
        {
            var roleIds = new List<int>();
            foreach (var user in users)
            {
                foreach (var roleId in user.Roles.Select(x => x.RoleId))
                {
                    roleIds.AddIfNotContains(roleId);
                }
            }

            var roles = _roleManager.Roles.Where(x => roleIds.Contains(x.Id));
            //return Mapper.Map<List<UserDto.RoleDto>>(roles);
            return roles;
        }

        public bool IsERM()
        {
            var user = _userManager.Users.Where(u => u.Id == AbpSession.UserId).FirstOrDefault();
            var userId = user?.Id;

            var isAdmin = _userManager.IsInRoleAsync(user, "Admin").Result;
            var isUser = _userManager.IsInRoleAsync(user, "User").Result;


            var rolesDump = _roleRepository.GetAll().Where(r => r.IsDeleted == false);

            var roleData = from r in rolesDump
                           select new
                           {
                               r.Id,
                               r.Name,
                               r.DisplayName
                           };

            var userRolesList = _userManager.GetRolesAsync(user).Result;


            var userRolesNames = new List<string>();
            foreach (var role in roleData)
            {
                if (userRolesList.Contains(role.Name))
                {
                    userRolesNames.Add(role.DisplayName.ToUpper());
                }
            }

            return userRolesNames.Contains("ERM");
        }

    }
}