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
using Abp.Domain.Repositories;
using CCPDemo.RiskTypes;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using CCPDemo.Authorization.Users;
using System.Linq;
using PayPalCheckoutSdk.Orders;
using CCPDemo.RiskTransactions.Dtos;
using CCPDemo.RiskTransactions;
using CCPDemo.VRisks.Dtos;
using CCPDemo.VRisks;
using CCPDemo.Web.Areas.App.Models.VRisks;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Risks)]
    public class RisksController : CCPDemoControllerBase
    {
        private readonly IRisksAppService _risksAppService;
        private readonly IRepository<RiskType> _riskTypeRepo;
        private readonly UserManager _userManager;
        private readonly IRepository<RiskTransaction> _riskTransactionRepository;

        public RisksController(IRisksAppService risksAppService, IRepository<RiskType> riskTypeRepo, UserManager userManager, IRepository<RiskTransaction> riskTransactionRepository)
        {
            _risksAppService = risksAppService;
            _riskTypeRepo = riskTypeRepo;
            _userManager = userManager;
            _riskTransactionRepository = riskTransactionRepository;
        }

        public ActionResult Index()
        {
            var user = _userManager.Users.Where(u => u.Id == AbpSession.UserId).FirstOrDefault();
            var roles = user.Roles;
            var isAdmin = _userManager.IsInRoleAsync(user, "Admin").Result ? "Admin" : "User";
            var isUser = _userManager.IsInRoleAsync(user, "User").Result ? "User" : "Admin";
            var isSupervisor = _userManager.IsInRoleAsync(user, "Supervisor").Result ? "Supervisor" : "Not a Supervisor";

            var isERM = _userManager.IsInRoleAsync(user, "ERM").Result;


            var model = new RisksViewModel
            {
                FilterText = "",
                User = user,
            };

            //if(!isERM)
            //{
                
            //    return View("~/Areas/App/Views/Risks/RiskOwner.cshtml", model);
            //}


            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Risks_Create, AppPermissions.Pages_Risks_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
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
            }

            var riskTypes = _riskTypeRepo.GetAll();
            var users = _userManager.Users.Where(u => u.IsActive == true).Where(u => u.IsDeleted == false).ToList();

            var selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem
            {
                Value = "",
                Text = "Select Risk Type"
            });
            foreach (var riskType in riskTypes)
            {
                selectListItems.Add(new SelectListItem
                {
                    Value = riskType.Name,
                    Text = riskType.Name
                });
            }

            var usersSelectList = new List<SelectListItem>();
            usersSelectList.Add(new SelectListItem
            {
                Value = "",
                Text = "Select Risk Owner"
            });

            foreach (var user in users)
            {
                usersSelectList.Add(new SelectListItem
                {
                    Value = Convert.ToString(user.Id),
                    Text = user.Name + $" ({user.EmailAddress})"
                });
            }



            var viewModel = new CreateOrEditRiskModalViewModel()
            {
                Risk = getRiskForEditOutput.Risk,
                RiskTypeList = selectListItems,
                UsersList = usersSelectList,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Risks_Create)]
        public async Task<PartialViewResult> ErmTransferRiskModal(int? id)
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
            }

            var loggedInUserId = AbpSession.UserId;

            var users = _userManager.Users.Where(u => u.IsActive == true).Where(u => u.IsDeleted == false).ToList();
            var user = _userManager.Users.Where(u => u.Id == loggedInUserId).First();

            var viewModel = new CreateOrEditRiskModalViewModel()
            {
                Risk = getRiskForEditOutput.Risk,
                Users = users,
                User = user,

            };

            return PartialView("_ErmTransferRiskModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Risks_Create)]
        public async Task<PartialViewResult> ErmUpDownRiskModal(int? id)
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
            }

            var loggedInUserId = AbpSession.UserId;

            var users = _userManager.Users.Where(u => u.IsActive == true).Where(u => u.IsDeleted == false).ToList();
            var user = _userManager.Users.Where(u => u.Id == loggedInUserId).First();

            var viewModel = new CreateOrEditRiskModalViewModel()
            {
                Risk = getRiskForEditOutput.Risk,
                //Users = users,
                //User = user,

            };

            return PartialView("_ErmUpDownRiskModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Risks_Create)]
        public async Task<PartialViewResult> ErmCloseRiskModal(int? id)
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
            }

            var loggedInUserId = AbpSession.UserId;

            var users = _userManager.Users.Where(u => u.IsActive == true).Where(u => u.IsDeleted == false).ToList();
            var user = _userManager.Users.Where(u => u.Id == loggedInUserId).First();
            var roles = user.Roles.ToString();


            var viewModel = new CreateOrEditRiskModalViewModel()
            {
                Risk = getRiskForEditOutput.Risk,
                Users = users,
                User = user,

            };

            return PartialView("_ErmCloseRiskModal", viewModel);
        }

        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Pages_Risks_Create)]
        public async Task<JsonResult> TransferRisk([FromBody] RiskTransactionDto model)
        {

            var riskTransactionDto = new RiskTransaction();
            riskTransactionDto.RiskId = model.RiskId;
            riskTransactionDto.CurrentValue = model.CurrentValue;
            riskTransactionDto.NewValue = model.NewValue;
            riskTransactionDto.UserId = model.UserId;
            riskTransactionDto.Date = model.Date;
            riskTransactionDto.TransactionType = model.TransactionType;



            var riskDto = new Risk();
            riskDto.Id = model.RiskId;
            riskDto.RiskOwner = model.NewValue;

            var upDateRiskDto = new CreateOrEditRiskDto();
            upDateRiskDto.Id = model.Id;
            upDateRiskDto.RiskOwner = model.NewValue;


            await _riskTransactionRepository.InsertAsync(riskTransactionDto);

            var user = _userManager.Users.Where(u => u.Id == model.UserId).FirstOrDefault();
            var roles = user.Roles;
            var isAdmin = _userManager.IsInRoleAsync(user, "Admin").Result ? "Admin" : "User";
            var isUser = _userManager.IsInRoleAsync(user, "User").Result ? "User" : "Admin";
            var isSupervisor = _userManager.IsInRoleAsync(user, "Supervisor").Result ? "Supervisor" : "Not a Supervisor";

            string[] userRoles = { isAdmin, isUser, isSupervisor };


            // Update risk owner
            //var riskUpdate = await _riskRepo.UpdateAsync(riskDto);
            //await _vRisksAppService.CreateOrEdit(upDateRiskDto);


            return Json(userRoles);

            //return View();
        }

        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Pages_Risks_Create)]
        public async Task<JsonResult> UpDownGradeRisk([FromBody] RiskTransactionDto model)
        {
            return Json(model);

            var riskTransactionDto = new RiskTransaction();
            riskTransactionDto.RiskId = model.RiskId;
            riskTransactionDto.CurrentValue = model.CurrentValue;
            riskTransactionDto.NewValue = model.NewValue;
            riskTransactionDto.UserId = model.UserId;
            riskTransactionDto.Date = model.Date;
            riskTransactionDto.TransactionType = model.TransactionType;



            var riskDto = new Risk();
            riskDto.Id = model.RiskId;
            riskDto.Rating = model.NewValue;

            var upDateRiskDto = new CreateOrEditRiskDto();
            upDateRiskDto.Id = model.Id;
            upDateRiskDto.RiskOwner = model.NewValue;


            await _riskTransactionRepository.InsertAsync(riskTransactionDto);

            //var user = _userManager.Users.Where(u => u.Id == model.UserId).FirstOrDefault();
            //var roles = user.Roles;
            //var isAdmin = _userManager.IsInRoleAsync(user, "Admin").Result ? "Admin" : "User";
            //var isUser = _userManager.IsInRoleAsync(user, "User").Result ? "User" : "Admin";
            //var isSupervisor = _userManager.IsInRoleAsync(user, "Supervisor").Result ? "Supervisor" : "Not a Supervisor";

            //string[] userRoles = { isAdmin, isUser, isSupervisor };


            // Update risk owner
            //var riskUpdate = await _riskRepo.UpdateAsync(riskDto);
            //await _vRisksAppService.CreateOrEdit(upDateRiskDto);


            return Json(model);

            //return View();
        }

        public async Task<PartialViewResult> ViewRiskModal(int id)
        {
            var getRiskForViewDto = await _risksAppService.GetRiskForView(id);

            var model = new RiskViewModel()
            {
                Risk = getRiskForViewDto.Risk
            };

            return PartialView("_ViewRiskModal", model);
        }

    }
}