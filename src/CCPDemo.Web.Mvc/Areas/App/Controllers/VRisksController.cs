using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCPDemo.Web.Areas.App.Models.VRisks;
using CCPDemo.Web.Controllers;
using CCPDemo.Authorization;
using CCPDemo.VRisks;
using CCPDemo.VRisks.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using CCPDemo.Authorization.Users;
using System.Linq;
using Abp.Net.Mail;
using System.Linq.Dynamic.Core;
using CCPDemo.RiskTransactions.Dtos;
using IdentityModel.OidcClient;
using CCPDemo.RiskTransactions;
using Abp.Runtime.Session;
using CCPDemo.Web.Areas.App.Models.Roles;
using Abp.Domain.Repositories;
using System.Collections.Generic;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_VRisks)]
    public class VRisksController : CCPDemoControllerBase
    {
        private readonly IVRisksAppService _vRisksAppService;
        private readonly UserManager _userManager;
        private readonly IRepository<RiskTransaction> _riskTransactionRepository;
        private readonly IRepository<VRisk> _riskRepo;


        public VRisksController(IVRisksAppService vRisksAppService, UserManager userManager, 
            IRepository<RiskTransaction> riskTransactionRepo, IRepository<VRisk> vRiskRepo)
        {
            _vRisksAppService = vRisksAppService;
            _userManager = userManager;
            _riskTransactionRepository= riskTransactionRepo;
            _riskRepo = vRiskRepo;

        }

        public ActionResult Index()
        {
            var model = new VRisksViewModel
            {
                FilterText = ""
            };

			return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_VRisks_Create, AppPermissions.Pages_VRisks_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetVRiskForEditOutput getVRiskForEditOutput;

            if (id.HasValue)
            {
                getVRiskForEditOutput = await _vRisksAppService.GetVRiskForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getVRiskForEditOutput = new GetVRiskForEditOutput
                {
                    VRisk = new CreateOrEditVRiskDto()
                };
            }

            var loggedInUserId = AbpSession.UserId;

            var users = _userManager.Users.Where(u => u.IsActive == true).Where(u => u.IsDeleted == false).ToList();
            var user = _userManager.Users.Where(u => u.Id == loggedInUserId).First();

            

            var viewModel = new CreateOrEditVRiskModalViewModel()
            {
                VRisk = getVRiskForEditOutput.VRisk,
                Users = users,
                User = user,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_VRisks_Create, AppPermissions.Pages_VRisks_Edit)]
        public async Task<PartialViewResult> ErmTransferRiskModal(int? id)
        {
            GetVRiskForEditOutput getVRiskForEditOutput;

            if (id.HasValue)
            {
                getVRiskForEditOutput = await _vRisksAppService.GetVRiskForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getVRiskForEditOutput = new GetVRiskForEditOutput
                {
                    VRisk = new CreateOrEditVRiskDto()
                };
            }

            var loggedInUserId = AbpSession.UserId;

            var users = _userManager.Users.Where(u => u.IsActive == true).Where(u => u.IsDeleted == false).ToList();
            var user = _userManager.Users.Where(u => u.Id == loggedInUserId).First();

            var viewModel = new CreateOrEditVRiskModalViewModel()
            {
                VRisk = getVRiskForEditOutput.VRisk,
                Users = users,
                User = user,

            };

            return PartialView("_ErmTransferRiskModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_VRisks_Create, AppPermissions.Pages_VRisks_Edit)]
        public async Task<PartialViewResult> ErmUpDownRiskModal(int? id)
        {
            GetVRiskForEditOutput getVRiskForEditOutput;

            if (id.HasValue)
            {
                getVRiskForEditOutput = await _vRisksAppService.GetVRiskForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getVRiskForEditOutput = new GetVRiskForEditOutput
                {
                    VRisk = new CreateOrEditVRiskDto()
                };
            }

            var loggedInUserId = AbpSession.UserId;

            var users = _userManager.Users.Where(u => u.IsActive == true).Where(u => u.IsDeleted == false).ToList();
            var user = _userManager.Users.Where(u => u.Id == loggedInUserId).First();

            var viewModel = new CreateOrEditVRiskModalViewModel()
            {
                VRisk = getVRiskForEditOutput.VRisk,
                Users = users,
                User = user,

            };

            return PartialView("_ErmUpDownRiskModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_VRisks_Create, AppPermissions.Pages_VRisks_Edit)]
        public async Task<PartialViewResult> ErmCloseRiskModal(int? id)
        {
            GetVRiskForEditOutput getVRiskForEditOutput;

            if (id.HasValue)
            {
                getVRiskForEditOutput = await _vRisksAppService.GetVRiskForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getVRiskForEditOutput = new GetVRiskForEditOutput
                {
                    VRisk = new CreateOrEditVRiskDto()
                };
            }

            var loggedInUserId = AbpSession.UserId;

            var users = _userManager.Users.Where(u => u.IsActive == true).Where(u => u.IsDeleted == false).ToList();
            var user = _userManager.Users.Where(u => u.Id == loggedInUserId).First();
            var roles = user.Roles.ToString();


            var viewModel = new CreateOrEditVRiskModalViewModel()
            {
                VRisk = getVRiskForEditOutput.VRisk,
                Users = users,
                User = user,

            };

            return PartialView("_ErmCloseRiskModal", viewModel);
        }

        [HttpPost]
        public async Task<JsonResult> TransferRisk([FromBody] RiskTransactionDto model)
        {

            var riskTransactionDto = new RiskTransaction();
            riskTransactionDto.RiskId = model.RiskId;
            riskTransactionDto.CurrentValue = model.CurrentValue;
            riskTransactionDto.NewValue = model.NewValue;
            riskTransactionDto.UserId = model.UserId;
            riskTransactionDto.Date= model.Date;
            riskTransactionDto.TransactionType= model.TransactionType;

           

            var riskDto = new VRisk();
            riskDto.Id= model.RiskId;
            riskDto.RiskOwner = model.NewValue;

            var upDateRiskDto = new CreateOrEditVRiskDto();
            upDateRiskDto.Id= model.Id;
            upDateRiskDto.RiskOwner = model.NewValue;  


			await _riskTransactionRepository.InsertAsync(riskTransactionDto);

			var user = _userManager.Users.Where(u => u.Id == model.UserId).FirstOrDefault();
            var roles = user.Roles;
            var isAdmin = _userManager.IsInRoleAsync(user, "Admin").Result ? "Admin" : "User";
            var isUser = _userManager.IsInRoleAsync(user, "User").Result ? "User" : "Admin";
            var isSupervisor = _userManager.IsInRoleAsync(user, "Supervisor").Result ? "Supervisor" : "Not a Supervisor";

            string[] userRoles = {isAdmin, isUser, isSupervisor};


			// Update risk owner
			//var riskUpdate = await _riskRepo.UpdateAsync(riskDto);
			//await _vRisksAppService.CreateOrEdit(upDateRiskDto);


			return Json(userRoles);

            //return View();
        }


        public async Task<PartialViewResult> ViewVRiskModal(int id)
        {
            var getVRiskForViewDto = await _vRisksAppService.GetVRiskForView(id);

            var model = new VRiskViewModel()
            {
                VRisk = getVRiskForViewDto.VRisk
            };

            return PartialView("_ViewVRiskModal", model);
        }

    }
}