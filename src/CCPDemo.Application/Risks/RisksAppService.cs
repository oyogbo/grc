using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.Risks.Exporting;
using CCPDemo.Risks.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;
using CCPDemo.Authorization.Users;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace CCPDemo.Risks
{
    [AbpAuthorize(AppPermissions.Pages_Risks)]
    public class RisksAppService : CCPDemoAppServiceBase, IRisksAppService
    {
        private readonly IRepository<Risk> _riskRepository;
        private readonly IRisksExcelExporter _risksExcelExporter;
        private readonly UserManager _userManager;
        private readonly ILogger _logger;

        public RisksAppService(IRepository<Risk> riskRepository, IRisksExcelExporter risksExcelExporter, UserManager userManager, ILogger<Risk> logger)
        {
            _riskRepository = riskRepository;
            _risksExcelExporter = risksExcelExporter;
            _userManager = userManager;
            _logger = logger;

        }

        public async Task<PagedResultDto<GetRiskForViewDto>> GetAll(GetAllRisksInput input)
        {

            var filteredRisks = _riskRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.RiskType.Contains(input.Filter) || e.RiskSummary.Contains(input.Filter) || e.Department.Contains(input.Filter) || e.RiskOwner.Contains(input.Filter) || e.ExistingControl.Contains(input.Filter) || e.ERMComment.Contains(input.Filter) || e.ActionPlan.Contains(input.Filter) || e.RiskOwnerComment.Contains(input.Filter) || e.Status.Contains(input.Filter) || e.Rating.Contains(input.Filter) || e.TargetDate.Contains(input.Filter) || e.ActualClosureDate.Contains(input.Filter) || e.AcceptanceDate.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskTypeFilter), e => e.RiskType == input.RiskTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskSummaryFilter), e => e.RiskSummary == input.RiskSummaryFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DepartmentFilter), e => e.Department == input.DepartmentFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskOwnerFilter), e => e.RiskOwner == input.RiskOwnerFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExistingControlFilter), e => e.ExistingControl == input.ExistingControlFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ERMCommentFilter), e => e.ERMComment == input.ERMCommentFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ActionPlanFilter), e => e.ActionPlan == input.ActionPlanFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskOwnerCommentFilter), e => e.RiskOwnerComment == input.RiskOwnerCommentFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusFilter), e => e.Status == input.StatusFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RatingFilter), e => e.Rating == input.RatingFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TargetDateFilter), e => e.TargetDate == input.TargetDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ActualClosureDateFilter), e => e.ActualClosureDate == input.ActualClosureDateFilter)
                        .WhereIf(input.RiskAcceptedFilter.HasValue && input.RiskAcceptedFilter > -1, e => (input.RiskAcceptedFilter == 1 && e.RiskAccepted) || (input.RiskAcceptedFilter == 0 && !e.RiskAccepted));

            var pagedAndFilteredRisks = filteredRisks
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var risks = from o in pagedAndFilteredRisks
                        select new
                        {

                            o.RiskType,
                            o.RiskSummary,
                            o.Department,
                            o.RiskOwner,
                            o.ExistingControl,
                            o.ERMComment,
                            o.ActionPlan,
                            o.RiskOwnerComment,
                            o.Status,
                            o.Rating,
                            o.TargetDate,
                            o.ActualClosureDate,
                            o.RiskAccepted,
                            o.RiskOwnerId,
                            Id = o.Id
                        };

            var totalCount = await filteredRisks.CountAsync();

            var dbList = await risks.ToListAsync();
            var results = new List<GetRiskForViewDto>();


            var user = _userManager.Users.Where(u => u.Id == AbpSession.UserId).FirstOrDefault();
            var roles = user.Roles;
            var isAdmin = _userManager.IsInRoleAsync(user, "Admin").Result;
            var isUser = _userManager.IsInRoleAsync(user, "User").Result ? "User" : "Admin";
            var isSupervisor = _userManager.IsInRoleAsync(user, "Supervisor").Result ? "Supervisor" : "Not a Supervisor";

            var isERM = _userManager.IsInRoleAsync(user, "ERM").Result;


            /***
             * Filter data presented according to Roles/ERM
             * 
             ***/


            foreach (var o in dbList)
            {
                if (!isERM || !isAdmin)
                {
                    if (o.RiskOwnerId == user.Id)
                    {
                        var res = new GetRiskForViewDto()
                        {
                            Risk = new RiskDto
                            {

                                RiskType = o.RiskType,
                                RiskSummary = o.RiskSummary,
                                Department = o.Department,
                                RiskOwner = o.RiskOwner,
                                ExistingControl = o.ExistingControl,
                                ERMComment = o.ERMComment,
                                ActionPlan = o.ActionPlan,
                                RiskOwnerComment = o.RiskOwnerComment,
                                Status = o.Status,
                                Rating = o.Rating,
                                TargetDate = o.TargetDate,
                                ActualClosureDate = o.ActualClosureDate,
                                RiskAccepted = o.RiskAccepted,
                                Id = o.Id,
                            }
                        };
                        results.Add(res);
                    }

                }
                else
                {
                    var res = new GetRiskForViewDto()
                    {
                        Risk = new RiskDto
                        {

                            RiskType = o.RiskType,
                            RiskSummary = o.RiskSummary,
                            Department = o.Department,
                            RiskOwner = o.RiskOwner,
                            ExistingControl = o.ExistingControl,
                            ERMComment = o.ERMComment,
                            ActionPlan = o.ActionPlan,
                            RiskOwnerComment = o.RiskOwnerComment,
                            Status = o.Status,
                            Rating = o.Rating,
                            TargetDate = o.TargetDate,
                            ActualClosureDate = o.ActualClosureDate,
                            RiskAccepted = o.RiskAccepted,
                            Id = o.Id,
                        }
                    };

                    results.Add(res);
                }

                
            }

            return new PagedResultDto<GetRiskForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetRiskForViewDto> GetRiskForView(int id)
        {
            var risk = await _riskRepository.GetAsync(id);

            var output = new GetRiskForViewDto { Risk = ObjectMapper.Map<RiskDto>(risk) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Risks_Edit)]
        public async Task<GetRiskForEditOutput> GetRiskForEdit(EntityDto input)
        {
            var risk = await _riskRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRiskForEditOutput { Risk = ObjectMapper.Map<CreateOrEditRiskDto>(risk) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditRiskDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Risks_Create)]
        protected virtual async Task Create(CreateOrEditRiskDto input)
        {

            var riskOwnerId = Convert.ToInt64(input.RiskOwner);
            var userName = _userManager.Users.Where(u=>u.Id == riskOwnerId).FirstOrDefault().FullName;

            input.RiskOwnerId = riskOwnerId;
            input.RiskOwner = userName;


            var risk = ObjectMapper.Map<Risk>(input);

            await _riskRepository.InsertAsync(risk);

        }

        [AbpAuthorize(AppPermissions.Pages_Risks_Edit)]
        protected virtual async Task Update(CreateOrEditRiskDto input)
        {


            var risk = await _riskRepository.GetAsync(Convert.ToInt32(input.Id));

            long riskOwnerId = Convert.ToInt64(input.RiskOwner);
            var userName = _userManager.Users.Where(u => u.Id == riskOwnerId).FirstOrDefault().FullName;

            input.RiskOwnerId = riskOwnerId;
            input.RiskOwner = userName;

            //risk.RiskType = risk.RiskType.IsNullOrEmpty() ? input.RiskType: risk.RiskType;
            risk.RiskType = risk.RiskType ?? risk.RiskType;
            risk.RiskSummary = risk.RiskSummary ?? input.RiskSummary;
            risk.RiskOwner = risk.RiskOwner ?? input.RiskOwner;
            risk.RiskOwnerId =  input.RiskOwnerId;
            risk.RiskOwnerComment = input.RiskOwnerComment;
            risk.AcceptanceDate = new DateTime().ToShortDateString().ToString();
            risk.ActionPlan = input.ActionPlan;
            risk.ExistingControl = input.ExistingControl;
            risk.ActualClosureDate = input.ActualClosureDate;
            risk.Department = input.Department;
            risk.ERMComment= input.ERMComment;
            risk.RiskAccepted = input.RiskAccepted;
            risk.RiskOwner = input.RiskOwner;
            risk.Rating = input.Rating;
            risk.Status= input.Status;

            await _riskRepository.UpdateAsync(risk);

            //var risk = await _riskRepository.FirstOrDefaultAsync((int)input.Id);
            //ObjectMapper.Map(input, risk);

        }

        [AbpAuthorize(AppPermissions.Pages_Risks_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _riskRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetRisksToExcel(GetAllRisksForExcelInput input)
        {

            var filteredRisks = _riskRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.RiskType.Contains(input.Filter) || e.RiskSummary.Contains(input.Filter) || e.Department.Contains(input.Filter) || e.RiskOwner.Contains(input.Filter) || e.ExistingControl.Contains(input.Filter) || e.ERMComment.Contains(input.Filter) || e.ActionPlan.Contains(input.Filter) || e.RiskOwnerComment.Contains(input.Filter) || e.Status.Contains(input.Filter) || e.Rating.Contains(input.Filter) || e.TargetDate.Contains(input.Filter) || e.ActualClosureDate.Contains(input.Filter) || e.AcceptanceDate.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskTypeFilter), e => e.RiskType == input.RiskTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskSummaryFilter), e => e.RiskSummary == input.RiskSummaryFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DepartmentFilter), e => e.Department == input.DepartmentFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskOwnerFilter), e => e.RiskOwner == input.RiskOwnerFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExistingControlFilter), e => e.ExistingControl == input.ExistingControlFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ERMCommentFilter), e => e.ERMComment == input.ERMCommentFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ActionPlanFilter), e => e.ActionPlan == input.ActionPlanFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskOwnerCommentFilter), e => e.RiskOwnerComment == input.RiskOwnerCommentFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusFilter), e => e.Status == input.StatusFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RatingFilter), e => e.Rating == input.RatingFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TargetDateFilter), e => e.TargetDate == input.TargetDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ActualClosureDateFilter), e => e.ActualClosureDate == input.ActualClosureDateFilter)
                        .WhereIf(input.RiskAcceptedFilter.HasValue && input.RiskAcceptedFilter > -1, e => (input.RiskAcceptedFilter == 1 && e.RiskAccepted) || (input.RiskAcceptedFilter == 0 && !e.RiskAccepted));

            var query = (from o in filteredRisks
                         select new GetRiskForViewDto()
                         {
                             Risk = new RiskDto
                             {
                                 RiskType = o.RiskType,
                                 RiskSummary = o.RiskSummary,
                                 Department = o.Department,
                                 RiskOwner = o.RiskOwner,
                                 ExistingControl = o.ExistingControl,
                                 ERMComment = o.ERMComment,
                                 ActionPlan = o.ActionPlan,
                                 RiskOwnerComment = o.RiskOwnerComment,
                                 Status = o.Status,
                                 Rating = o.Rating,
                                 TargetDate = o.TargetDate,
                                 ActualClosureDate = o.ActualClosureDate,
                                 RiskAccepted = o.RiskAccepted,
                                 Id = o.Id
                             }
                         });

            var riskListDtos = await query.ToListAsync();

            return _risksExcelExporter.ExportToFile(riskListDtos);
        }

    }
}