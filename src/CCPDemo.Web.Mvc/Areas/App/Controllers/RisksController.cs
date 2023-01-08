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

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Risks)]
    public class RisksController : CCPDemoControllerBase
    {
        private readonly IRisksAppService _risksAppService;
        private readonly IRepository<RiskTransaction> _riskTransactionRepository;
        private readonly IRepository<Risk> _riskRepository;

        public RisksController(IRisksAppService risksAppService, IRepository<Risk> riskRepository, IRepository<RiskTransaction> riskTransactionRepository)
        {
            _risksAppService = risksAppService;
            _riskRepository = riskRepository;
            _riskTransactionRepository = riskTransactionRepository;
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

            };

            return PartialView("_ErmCloseRisk", viewModel);
        }

        public async Task<PartialViewResult> ViewRiskModal(int id)
        {
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
                UserName = getRiskForViewDto.UserName

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

    }
}