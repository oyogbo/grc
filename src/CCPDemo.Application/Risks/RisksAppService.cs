using CCPDemo.RiskTypes;
using Abp.Organizations;
using CCPDemo.RiskStatus;
using CCPDemo.RiskRatings;
using CCPDemo.Authorization.Users;
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
using CCPDemo.RiskTransactions.Dtos;
using CCPDemo.RiskTransactions;
using Microsoft.AspNetCore.Mvc;
using Twilio.Rest.Trunking.V1;
using Abp.Authorization.Users;
using CCPDemo.EntityFrameworkCore;
using Abp.EntityFrameworkCore;
using CCPDemo.Authorization.Roles;
using CCPDemo.Organizations.Dto;
using Newtonsoft.Json;
using CCPDemo.KeyRiskIndicators.Dtos;
using System.Net.Mail;
using System.Net;
using Abp.Net.Mail;
using CCPDemo.Configuration.Host.Dto;
using System.Text;
using CCPDemo.Net.Emailing;
using CCPDemo.RiskRatings.Dtos;

namespace CCPDemo.Risks
{
    [AbpAuthorize(AppPermissions.Pages_Risks)]
    public class RisksAppService : CCPDemoAppServiceBase, IRisksAppService
    {
        private readonly IRepository<Risk> _riskRepository;
        private readonly IRepository<RiskTransaction> _riskTransactionRepository;
        private readonly IRisksExcelExporter _risksExcelExporter;
        private readonly IRepository<RiskType, int> _lookup_riskTypeRepository;
        private readonly IRepository<OrganizationUnit, long> _lookup_organizationUnitRepository;
        private readonly IRepository<Status, int> _lookup_statusRepository;
        private readonly IRepository<RiskRating, int> _lookup_riskRatingRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly UserManager _userManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IEmailSender _emailSender;
        private readonly IEmailTemplateProvider _emailTemplateProvider;


        private string _emailButtonStyle =
            "padding-left: 30px; padding-right: 30px; padding-top: 12px; padding-bottom: 12px; color: #ffffff; background-color: #00bb77; font-size: 14pt; text-decoration: none;";

        private string _emailButtonColor = "#00bb77";

        //private readonly CCPDemoDbContext _ctx;

        public RisksAppService(IRepository<Risk> riskRepository,
            IEmailTemplateProvider emailTemplateProvider,
            IRisksExcelExporter risksExcelExporter, 
            IRepository<RiskType, int> lookup_riskTypeRepository, 
            IRepository<OrganizationUnit, long> lookup_organizationUnitRepository,
            IRepository<Status, int> lookup_statusRepository, 
            IRepository<RiskRating, int> lookup_riskRatingRepository, 
            IRepository<User, long> lookup_userRepository, UserManager userManager,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<Role> roleRepository,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
             IEmailSender emailSender
            /*IDbContextProvider<CCPDemoDbContext> dbContextProvider*/)
        {
            _riskRepository = riskRepository;
            _risksExcelExporter = risksExcelExporter;
            _lookup_riskTypeRepository = lookup_riskTypeRepository;
            _lookup_organizationUnitRepository = lookup_organizationUnitRepository;
            _lookup_statusRepository = lookup_statusRepository;
            _lookup_riskRatingRepository = lookup_riskRatingRepository;
            _lookup_userRepository = lookup_userRepository;
            _userManager = userManager;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            //_ctx = dbContextProvider.GetDbContext();
            _roleRepository = roleRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _emailSender = emailSender;
            _emailTemplateProvider = emailTemplateProvider;
        }

        public async Task<PagedResultDto<GetRiskForViewDto>> GetAll(GetAllRisksInput input)
        {

            var filteredRisks = _riskRepository.GetAll()
                        .Include(e => e.RiskTypeFk)
                        .Include(e => e.OrganizationUnitFk)
                        .Include(e => e.StatusFk)
                        .Include(e => e.RiskRatingFk)
                        .Include(e => e.UserFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Summary.Contains(input.Filter) || e.ExistingControl.Contains(input.Filter) || e.ERMRecommendation.Contains(input.Filter) || e.ActionPlan.Contains(input.Filter) || e.RiskOwnerComment.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SummaryFilter), e => e.Summary == input.SummaryFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExistingControlFilter), e => e.ExistingControl == input.ExistingControlFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ERMRecommendationFilter), e => e.ERMRecommendation == input.ERMRecommendationFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ActionPlanFilter), e => e.ActionPlan == input.ActionPlanFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskOwnerCommentFilter), e => e.RiskOwnerComment == input.RiskOwnerCommentFilter)
                        .WhereIf(input.MinTargetDateFilter != null, e => e.TargetDate >= input.MinTargetDateFilter)
                        .WhereIf(input.MaxTargetDateFilter != null, e => e.TargetDate <= input.MaxTargetDateFilter)
                        .WhereIf(input.MinActualClosureDateFilter != null, e => e.ActualClosureDate >= input.MinActualClosureDateFilter)
                        .WhereIf(input.MaxActualClosureDateFilter != null, e => e.ActualClosureDate <= input.MaxActualClosureDateFilter)
                        .WhereIf(input.MinAcceptanceDateFilter != null, e => e.AcceptanceDate >= input.MinAcceptanceDateFilter)
                        .WhereIf(input.MaxAcceptanceDateFilter != null, e => e.AcceptanceDate <= input.MaxAcceptanceDateFilter)
                        .WhereIf(input.RiskAcceptedFilter.HasValue && input.RiskAcceptedFilter > -1, e => (input.RiskAcceptedFilter == 1 && e.RiskAccepted) || (input.RiskAcceptedFilter == 0 && !e.RiskAccepted))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskTypeNameFilter), e => e.RiskTypeFk != null && e.RiskTypeFk.Name == input.RiskTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusNameFilter), e => e.StatusFk != null && e.StatusFk.Name == input.StatusNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskRatingNameFilter), e => e.RiskRatingFk != null && e.RiskRatingFk.Name == input.RiskRatingNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var pagedAndFilteredRisks = filteredRisks
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var risks = from o in pagedAndFilteredRisks
                        join o1 in _lookup_riskTypeRepository.GetAll() on o.RiskTypeId equals o1.Id into j1
                        from s1 in j1.DefaultIfEmpty()

                        join o2 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o2.Id into j2
                        from s2 in j2.DefaultIfEmpty()

                        join o3 in _lookup_statusRepository.GetAll() on o.StatusId equals o3.Id into j3
                        from s3 in j3.DefaultIfEmpty()

                        join o4 in _lookup_riskRatingRepository.GetAll() on o.RiskRatingId equals o4.Id into j4
                        from s4 in j4.DefaultIfEmpty()

                        join o5 in _lookup_userRepository.GetAll() on o.UserId equals o5.Id into j5
                        from s5 in j5.DefaultIfEmpty()

                        select new
                        {

                            o.Summary,
                            o.ExistingControl,
                            o.ERMRecommendation,
                            o.ActionPlan,
                            o.RiskOwnerComment,
                            o.TargetDate,
                            o.ActualClosureDate,
                            o.AcceptanceDate,
                            o.RiskAccepted,
                            Id = o.Id,
                            o.UserId,
                            RiskTypeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                            OrganizationUnitDisplayName = s2 == null || s2.DisplayName == null ? "" : s2.DisplayName.ToString(),
                            StatusName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                            RiskRatingName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                            UserName = s5 == null || s5.Name == null ? "" : s5.Name.ToString()
                        };

            var totalCount = await filteredRisks.CountAsync();

            var dbList = await risks.ToListAsync();
            var results = new List<GetRiskForViewDto>();


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

            var isERM = userRolesNames.Contains("ERM");


            foreach (var o in dbList)
            {
                if (isAdmin || isERM)
                {
                    var res = new GetRiskForViewDto()
                    {
                        Risk = new RiskDto
                        {

                            Summary = o.Summary,
                            ExistingControl = o.ExistingControl,
                            ERMRecommendation = o.ERMRecommendation,
                            ActionPlan = o.ActionPlan,
                            RiskOwnerComment = o.RiskOwnerComment,
                            TargetDate = o.TargetDate,
                            ActualClosureDate = o.ActualClosureDate,
                            AcceptanceDate = o.AcceptanceDate,
                            RiskAccepted = o.RiskAccepted,
                            Id = o.Id,
                        },
                        RiskTypeName = o.RiskTypeName,
                        OrganizationUnitDisplayName = o.OrganizationUnitDisplayName,
                        StatusName = o.StatusName,
                        RiskRatingName = o.RiskRatingName,
                        UserName = o.UserName
                    };

                    results.Add(res);
                }
                else
                {
                    if(o.UserId == userId)
                    {
                        var res = new GetRiskForViewDto()
                        {
                            Risk = new RiskDto
                            {

                                Summary = o.Summary,
                                ExistingControl = o.ExistingControl,
                                ERMRecommendation = o.ERMRecommendation,
                                ActionPlan = o.ActionPlan,
                                RiskOwnerComment = o.RiskOwnerComment,
                                TargetDate = o.TargetDate,
                                ActualClosureDate = o.ActualClosureDate,
                                AcceptanceDate = o.AcceptanceDate,
                                RiskAccepted = o.RiskAccepted,
                                Id = o.Id,
                            },
                            RiskTypeName = o.RiskTypeName,
                            OrganizationUnitDisplayName = o.OrganizationUnitDisplayName,
                            StatusName = o.StatusName,
                            RiskRatingName = o.RiskRatingName,
                            UserName = o.UserName
                        };

                        results.Add(res);
                    }
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

            if (output.Risk.RiskTypeId != null)
            {
                var _lookupRiskType = await _lookup_riskTypeRepository.FirstOrDefaultAsync((int)output.Risk.RiskTypeId);
                output.RiskTypeName = _lookupRiskType?.Name?.ToString();
            }

            if (output.Risk.OrganizationUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.Risk.OrganizationUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit?.DisplayName?.ToString();
            }

            if (output.Risk.StatusId != null)
            {
                var _lookupStatus = await _lookup_statusRepository.FirstOrDefaultAsync((int)output.Risk.StatusId);
                output.StatusName = _lookupStatus?.Name?.ToString();
            }

            if (output.Risk.RiskRatingId != null)
            {
                var _lookupRiskRating = await _lookup_riskRatingRepository.FirstOrDefaultAsync((int)output.Risk.RiskRatingId);
                output.RiskRatingName = _lookupRiskRating?.Name?.ToString();
            }

            if (output.Risk.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Risk.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Risks_Edit)]
        public async Task<GetRiskForEditOutput> GetRiskForEdit(EntityDto input)
        {
            var risk = await _riskRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRiskForEditOutput { Risk = ObjectMapper.Map<CreateOrEditRiskDto>(risk) };

            if (output.Risk.RiskTypeId != null)
            {
                var _lookupRiskType = await _lookup_riskTypeRepository.FirstOrDefaultAsync((int)output.Risk.RiskTypeId);
                output.RiskTypeName = _lookupRiskType?.Name?.ToString();
            }

            if (output.Risk.OrganizationUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.Risk.OrganizationUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit?.DisplayName?.ToString();
            }

            if (output.Risk.StatusId != null)
            {
                var _lookupStatus = await _lookup_statusRepository.FirstOrDefaultAsync((int)output.Risk.StatusId);
                output.StatusName = _lookupStatus?.Name?.ToString();
            }

            if (output.Risk.RiskRatingId != null)
            {
                var _lookupRiskRating = await _lookup_riskRatingRepository.FirstOrDefaultAsync((int)output.Risk.RiskRatingId);
                output.RiskRatingName = _lookupRiskRating?.Name?.ToString();
            }

            if (output.Risk.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Risk.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditRiskDto input)
        {
            if (input.Id == null)
            {
                input.AcceptanceDate = null;
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
            var isErm = IsERM();
            if (isErm)
            {
                input.TargetDate = null;
            }

            var risk = ObjectMapper.Map<Risk>(input);

            await _riskRepository.InsertAsync(risk);

        }

        [AbpAuthorize(AppPermissions.Pages_Risks_Edit)]
        protected virtual async Task Update(CreateOrEditRiskDto input)
        {
            var isErm = IsERM();
            if (!isErm)
            {
                input.AcceptanceDate = DateTime.Now;
            }
            var risk = await _riskRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, risk);

        }

        [AbpAuthorize(AppPermissions.Pages_Risks_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _riskRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetRisksToExcel(GetAllRisksForExcelInput input)
        {

            var filteredRisks = _riskRepository.GetAll()
                        .Include(e => e.RiskTypeFk)
                        .Include(e => e.OrganizationUnitFk)
                        .Include(e => e.StatusFk)
                        .Include(e => e.RiskRatingFk)
                        .Include(e => e.UserFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Summary.Contains(input.Filter) || e.ExistingControl.Contains(input.Filter) || e.ERMRecommendation.Contains(input.Filter) || e.ActionPlan.Contains(input.Filter) || e.RiskOwnerComment.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SummaryFilter), e => e.Summary == input.SummaryFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExistingControlFilter), e => e.ExistingControl == input.ExistingControlFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ERMRecommendationFilter), e => e.ERMRecommendation == input.ERMRecommendationFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ActionPlanFilter), e => e.ActionPlan == input.ActionPlanFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskOwnerCommentFilter), e => e.RiskOwnerComment == input.RiskOwnerCommentFilter)
                        .WhereIf(input.MinTargetDateFilter != null, e => e.TargetDate >= input.MinTargetDateFilter)
                        .WhereIf(input.MaxTargetDateFilter != null, e => e.TargetDate <= input.MaxTargetDateFilter)
                        .WhereIf(input.MinActualClosureDateFilter != null, e => e.ActualClosureDate >= input.MinActualClosureDateFilter)
                        .WhereIf(input.MaxActualClosureDateFilter != null, e => e.ActualClosureDate <= input.MaxActualClosureDateFilter)
                        .WhereIf(input.MinAcceptanceDateFilter != null, e => e.AcceptanceDate >= input.MinAcceptanceDateFilter)
                        .WhereIf(input.MaxAcceptanceDateFilter != null, e => e.AcceptanceDate <= input.MaxAcceptanceDateFilter)
                        .WhereIf(input.RiskAcceptedFilter.HasValue && input.RiskAcceptedFilter > -1, e => (input.RiskAcceptedFilter == 1 && e.RiskAccepted) || (input.RiskAcceptedFilter == 0 && !e.RiskAccepted))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskTypeNameFilter), e => e.RiskTypeFk != null && e.RiskTypeFk.Name == input.RiskTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusNameFilter), e => e.StatusFk != null && e.StatusFk.Name == input.StatusNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskRatingNameFilter), e => e.RiskRatingFk != null && e.RiskRatingFk.Name == input.RiskRatingNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var query = (from o in filteredRisks
                         join o1 in _lookup_riskTypeRepository.GetAll() on o.RiskTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_statusRepository.GetAll() on o.StatusId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_riskRatingRepository.GetAll() on o.RiskRatingId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         join o5 in _lookup_userRepository.GetAll() on o.UserId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()

                         select new GetRiskForViewDto()
                         {
                             Risk = new RiskDto
                             {
                                 Summary = o.Summary,
                                 ExistingControl = o.ExistingControl,
                                 ERMRecommendation = o.ERMRecommendation,
                                 ActionPlan = o.ActionPlan,
                                 RiskOwnerComment = o.RiskOwnerComment,
                                 TargetDate = o.TargetDate,
                                 ActualClosureDate = o.ActualClosureDate,
                                 AcceptanceDate = o.AcceptanceDate,
                                 RiskAccepted = o.RiskAccepted,
                                 Id = o.Id
                             },
                             RiskTypeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             OrganizationUnitDisplayName = s2 == null || s2.DisplayName == null ? "" : s2.DisplayName.ToString(),
                             StatusName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             RiskRatingName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                             UserName = s5 == null || s5.Name == null ? "" : s5.Name.ToString()
                         });

            var riskListDtos = await query.ToListAsync();

            return _risksExcelExporter.ExportToFile(riskListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Risks)]
        public async Task<List<RiskRiskTypeLookupTableDto>> GetAllRiskTypeForTableDropdown()
        {
            return await _lookup_riskTypeRepository.GetAll()
                .Select(riskType => new RiskRiskTypeLookupTableDto
                {
                    Id = riskType.Id,
                    DisplayName = riskType == null || riskType.Name == null ? "" : riskType.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Risks)]
        public async Task<List<RiskOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForTableDropdown()
        {
            return await _lookup_organizationUnitRepository.GetAll()
                .Select(organizationUnit => new RiskOrganizationUnitLookupTableDto
                {
                    Id = organizationUnit.Id,
                    DisplayName = organizationUnit == null || organizationUnit.DisplayName == null ? "" : organizationUnit.DisplayName.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Risks)]
        public async Task<List<RiskStatusLookupTableDto>> GetAllStatusForTableDropdown()
        {
            return await _lookup_statusRepository.GetAll()
                .Select(status => new RiskStatusLookupTableDto
                {
                    Id = status.Id,
                    DisplayName = status == null || status.Name == null ? "" : status.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Risks)]
        public async Task<List<RiskRiskRatingLookupTableDto>> GetAllRiskRatingForTableDropdown()
        {
            return await _lookup_riskRatingRepository.GetAll()
                .Select(riskRating => new RiskRiskRatingLookupTableDto
                {
                    Id = riskRating.Id,
                    DisplayName = riskRating == null || riskRating.Name == null ? "" : riskRating.Name.ToString()
                }).ToListAsync();
        }

        public async Task<ListResultDto<RiskRatingDto>> GetRatings(GetAllRiskRatingsInput input)
        {
            var ratings = await _lookup_riskRatingRepository.GetAll()
                .Select(riskRating => new RiskRiskRatingLookupTableDto
                {
                    Id = riskRating.Id,
                    DisplayName = riskRating == null || riskRating.Name == null ? "" : riskRating.Name.ToString()
                }).ToListAsync();

            return new ListResultDto<RiskRatingDto>(ObjectMapper.Map<List<RiskRatingDto>>(ratings));
        }

        [AbpAuthorize(AppPermissions.Pages_Risks)]
        public async Task<List<RiskUserLookupTableDto>> GetAllUserForTableDropdown()
        {
            return await _lookup_userRepository.GetAll()
                .Select(user => new RiskUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user == null || user.Name == null ? "" : user.Name.ToString()
                }).ToListAsync();
        }

        public async Task TransferRisk(CreateOrEditRiskRiskTransactionDto input)
        {
            // Consequently, Raise a New Risk

            var riskCreateDto = new CreateOrEditRiskDto();
            riskCreateDto.Summary = input.Summary;
            riskCreateDto.TargetDate = input.TargetDate;
            riskCreateDto.AcceptanceDate = input.AcceptanceDate;
            riskCreateDto.ActualClosureDate = input.ActualClosureDate;
            riskCreateDto.ActionPlan = input.ActionPlan;
            riskCreateDto.OrganizationUnitId = input.OrganizationUnitId;
            riskCreateDto.StatusId = input.StatusId;
            riskCreateDto.ERMRecommendation = input.ERMRecommendation;
            riskCreateDto.ExistingControl = input.ExistingControl;
            riskCreateDto.RiskAccepted = input.RiskAccepted;
            riskCreateDto.RiskOwnerComment = input.RiskOwnerComment;
            riskCreateDto.RiskRatingId = input.RiskRatingId;
            riskCreateDto.RiskTypeId = input.RiskTypeId;
            riskCreateDto.UserId = input.UserId;


            var risk = ObjectMapper.Map<Risk>(input);

            await _riskRepository.InsertAsync(risk);


            // Create a Risk Transfer Record/History


            var riskTransactionDto = new RiskTransactionDto();
            riskTransactionDto.TransactionType = input.TransactionType;
            riskTransactionDto.RiskId = input.RiskId;
            riskTransactionDto.UserId = (long)input.UserId;
            riskTransactionDto.CurrentValue = input.CurrentValue;
            riskTransactionDto.NewValue = Convert.ToString(input.OrganizationUnitId);
            riskTransactionDto.Date = new DateTime().ToLongDateString();

            var riskTransaction = ObjectMapper.Map<RiskTransaction>(riskTransactionDto);

            await _riskTransactionRepository.InsertAsync(riskTransaction); ;
        }

        public string GetOrganizationalUnitName(int? id)
        {
            var unitName = _lookup_organizationUnitRepository.FirstOrDefault(x => x.Id == id).DisplayName;
            return unitName;
        }

        public IDictionary<int, string> GetAllRatings()
        {
            IDictionary<int, string> ratingsDict = new Dictionary<int, string>();

            var ratings = _lookup_riskRatingRepository.GetAll()
                .Select(riskRating => new RiskRiskRatingLookupTableDto
                {
                    Id = riskRating.Id,
                    DisplayName = riskRating == null || riskRating.Name == null ? "" : riskRating.Name.ToString()
                }).ToListAsync().Result;

            foreach(var rating in ratings) {
                ratingsDict.Add(rating.Id, rating.DisplayName);
            }

            return ratingsDict;
        }

        public async Task SaveRiskTransferData(ModifiedRiskTransactionsDto input)
        {

            var riskTransactionDto = new RiskTransaction
            {
                TransactionType = input.transactionType,
                RiskId = Convert.ToInt32(input.riskId),
                UserId = Convert.ToInt64(input.riskTransferrerUserId),
                CurrentValue = GetOrganizationalUnitName(input.currentOwner),
                NewValue = GetOrganizationalUnitName(input.organizationUnitId),
                Date = new DateTime().ToLongDateString()
            };

            var riskDto = new CreateOrEditRiskDto
            {
                RiskTypeId= input.riskTypeId,
                Summary = input.summary,
                OrganizationUnitId = input.organizationUnitId,
                ExistingControl = input.existingControl,
                ERMRecommendation = input.ermRecommendation,
                ActionPlan = input.actionPlan,
                RiskOwnerComment = input.riskOwnerComment,
                TargetDate = input.targetDate,
                ActualClosureDate= input.actualClosureDate,
                AcceptanceDate= input.acceptanceDate,
                RiskAccepted = input.riskAccepted,
                StatusId= input.statusId,
                RiskRatingId= input.riskRatingId,
                UserId= input.userId,
            };


            try
            {
                //var riskTransaction = ObjectMapper.Map<RiskTransaction>(riskTransactionDto);

                //await _riskTransactionRepository.InsertAsync(riskTransactionDto);

                //await _riskRepository.InsertAsync(riskDto);

                await Create(riskDto);

                //return "success";
            }
            catch(Exception ex)
            {
                //return ex.Message;
            }
        }

        public async Task<PagedResultDto<OrganizationUnitUserListDto>> UsersInOrganizationalUnit(int id)
        {
            //var userIdsInOrganizationUnit = _userOrganizationUnitRepository.GetAll()
            //    .Where(uou => uou.OrganizationUnitId == id);

            //var query = UserManager.Users
            //    .Where(u => !userIdsInOrganizationUnit.Contains(id));

            //////var query = UserManager.Users
            //////    .Where(u => u.OrganizationUnits.Contains((UserOrganizationUnit)userIdsInOrganizationUnit));

            //var userCount = await query.CountAsync();
            //var users = await query
            //    .OrderBy(u => u.Name)
            //    .ThenBy(u => u.Surname)
            //    .ToListAsync();

            //return new PagedResultDto<NameValueDto>(
            //    userCount,
            //    users.Select(u =>
            //        new NameValueDto(
            //            u.Name + " (" + u.EmailAddress + ")",
            //            u.Id.ToString()
            //        )
            //    ).ToList()
            //);

            var query = from ouUser in _userOrganizationUnitRepository.GetAll()
                        join ou in _organizationUnitRepository.GetAll() on ouUser.OrganizationUnitId equals ou.Id
                        join user in UserManager.Users on ouUser.UserId equals user.Id
                        where ouUser.OrganizationUnitId == id
                        select new
                        {
                            ouUser,
                            user
                        };
            var totalCount = await query.CountAsync();
            var items = await query.ToListAsync();
           

            return new PagedResultDto<OrganizationUnitUserListDto>(
                totalCount,
                items.Select(item =>
                {
                    var organizationUnitUserDto = ObjectMapper.Map<OrganizationUnitUserListDto>(item.user);
                    organizationUnitUserDto.AddedTime = item.ouUser.CreationTime;
                    //organizationUnitUserDto.AddedTime = item.ouUser.CreationTime;
                    return organizationUnitUserDto;
                }).ToList());

        }

        public int GetUserId()
        {
            return (int)AbpSession.UserId;
        }

        public ActionResult<string> UsersEmailsIdsDict()
        {
            var users = _userManager.Users.Where(u => u.IsDeleted == false).ToList();

            var usersQuery = from u in users
                             select new
                             {
                                 u.EmailAddress,
                                 u.Id
                             };

            var usersEmailIdsDict = new Dictionary<string, int>();
            foreach (var ueIds in usersQuery)
            {
                usersEmailIdsDict.Add(ueIds.EmailAddress, (int)ueIds.Id);
            }

            return JsonConvert.SerializeObject(usersEmailIdsDict);

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

        public bool IsRiskAccepted(int id)
        {
            var query = _riskRepository.GetAll().Where(r=>r.Id == id).Where(r=>r.UserId == AbpSession.UserId).Where(r => r.AcceptanceDate != null);
            var isRiskAccepted = (from r in query
                                        select new
                                        {
                                            r.RiskAccepted
                                        }).Any();

            return isRiskAccepted;
        }

        public async Task ResetAcceptance(CreateOrEditRiskDto input)
        {
            input.RiskAccepted = false;
            input.AcceptanceDate = null;
            await Update(input);
        }

        public async Task<PagedResultDto<GetRiskForViewDto>> OverDueRisks(GetAllRisksInput input)
        {
            var dateNow = DateTime.Now;

            var filteredRisks = _riskRepository.GetAll()
                        .Include(e => e.RiskTypeFk)
                        .Include(e => e.OrganizationUnitFk)
                        .Include(e => e.StatusFk)
                        .Include(e => e.RiskRatingFk)
                        .Include(e => e.UserFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Summary.Contains(input.Filter) || e.ExistingControl.Contains(input.Filter) || e.ERMRecommendation.Contains(input.Filter) || e.ActionPlan.Contains(input.Filter) || e.RiskOwnerComment.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SummaryFilter), e => e.Summary == input.SummaryFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExistingControlFilter), e => e.ExistingControl == input.ExistingControlFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ERMRecommendationFilter), e => e.ERMRecommendation == input.ERMRecommendationFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ActionPlanFilter), e => e.ActionPlan == input.ActionPlanFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskOwnerCommentFilter), e => e.RiskOwnerComment == input.RiskOwnerCommentFilter)
                        .WhereIf(input.MinTargetDateFilter != null, e => e.TargetDate >= input.MinTargetDateFilter)
                        .WhereIf(input.MaxTargetDateFilter != null, e => e.TargetDate <= input.MaxTargetDateFilter)
                        .WhereIf(input.MinActualClosureDateFilter != null, e => e.ActualClosureDate >= input.MinActualClosureDateFilter)
                        .WhereIf(input.MaxActualClosureDateFilter != null, e => e.ActualClosureDate <= input.MaxActualClosureDateFilter)
                        .WhereIf(input.MinAcceptanceDateFilter != null, e => e.AcceptanceDate >= input.MinAcceptanceDateFilter)
                        .WhereIf(input.MaxAcceptanceDateFilter != null, e => e.AcceptanceDate <= input.MaxAcceptanceDateFilter)
                        .WhereIf(input.RiskAcceptedFilter.HasValue && input.RiskAcceptedFilter > -1, e => (input.RiskAcceptedFilter == 1 && e.RiskAccepted) || (input.RiskAcceptedFilter == 0 && !e.RiskAccepted))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskTypeNameFilter), e => e.RiskTypeFk != null && e.RiskTypeFk.Name == input.RiskTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusNameFilter), e => e.StatusFk != null && e.StatusFk.Name == input.StatusNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskRatingNameFilter), e => e.RiskRatingFk != null && e.RiskRatingFk.Name == input.RiskRatingNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var pagedAndFilteredRisks = filteredRisks
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var risks = from o in pagedAndFilteredRisks
                        join o1 in _lookup_riskTypeRepository.GetAll() on o.RiskTypeId equals o1.Id into j1
                        from s1 in j1.DefaultIfEmpty()

                        join o2 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o2.Id into j2
                        from s2 in j2.DefaultIfEmpty()

                        join o3 in _lookup_statusRepository.GetAll() on o.StatusId equals o3.Id into j3
                        from s3 in j3.DefaultIfEmpty()

                        join o4 in _lookup_riskRatingRepository.GetAll() on o.RiskRatingId equals o4.Id into j4
                        from s4 in j4.DefaultIfEmpty()

                        join o5 in _lookup_userRepository.GetAll() on o.UserId equals o5.Id into j5
                        from s5 in j5.DefaultIfEmpty()

                        select new
                        {

                            o.Summary,
                            o.ExistingControl,
                            o.ERMRecommendation,
                            o.ActionPlan,
                            o.RiskOwnerComment,
                            o.TargetDate,
                            o.ActualClosureDate,
                            o.AcceptanceDate,
                            o.RiskAccepted,
                            Id = o.Id,
                            o.UserId,
                            RiskTypeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                            OrganizationUnitDisplayName = s2 == null || s2.DisplayName == null ? "" : s2.DisplayName.ToString(),
                            StatusName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                            RiskRatingName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                            UserName = s5 == null || s5.Name == null ? "" : s5.Name.ToString()
                        };

            var totalCount = await filteredRisks.CountAsync();

            var dbList = await risks.ToListAsync();
            var results = new List<GetRiskForViewDto>();

            foreach (var o in dbList)
            {
                if (o.TargetDate < dateNow && o.StatusName.ToUpper() != "CLOSE")
                {
                    var res = new GetRiskForViewDto()
                {
                    Risk = new RiskDto
                    {

                        Summary = o.Summary,
                        ExistingControl = o.ExistingControl,
                        ERMRecommendation = o.ERMRecommendation,
                        ActionPlan = o.ActionPlan,
                        RiskOwnerComment = o.RiskOwnerComment,
                        TargetDate = o.TargetDate,
                        ActualClosureDate = o.ActualClosureDate,
                        AcceptanceDate = o.AcceptanceDate,
                        RiskAccepted = o.RiskAccepted,
                        Id = o.Id,
                    },
                    RiskTypeName = o.RiskTypeName,
                    OrganizationUnitDisplayName = o.OrganizationUnitDisplayName,
                    StatusName = o.StatusName,
                    RiskRatingName = o.RiskRatingName,
                    UserName = o.UserName
                };

                results.Add(res);
                }
                

            }

            return new PagedResultDto<GetRiskForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<PagedResultDto<GetRiskForViewDto>> OnGoingRisks(GetAllRisksInput input)
        {
            var dateNow = DateTime.Now;

            var filteredRisks = _riskRepository.GetAll()
                        .Include(e => e.RiskTypeFk)
                        .Include(e => e.OrganizationUnitFk)
                        .Include(e => e.StatusFk)
                        .Include(e => e.RiskRatingFk)
                        .Include(e => e.UserFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Summary.Contains(input.Filter) || e.ExistingControl.Contains(input.Filter) || e.ERMRecommendation.Contains(input.Filter) || e.ActionPlan.Contains(input.Filter) || e.RiskOwnerComment.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SummaryFilter), e => e.Summary == input.SummaryFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExistingControlFilter), e => e.ExistingControl == input.ExistingControlFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ERMRecommendationFilter), e => e.ERMRecommendation == input.ERMRecommendationFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ActionPlanFilter), e => e.ActionPlan == input.ActionPlanFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskOwnerCommentFilter), e => e.RiskOwnerComment == input.RiskOwnerCommentFilter)
                        .WhereIf(input.MinTargetDateFilter != null, e => e.TargetDate >= input.MinTargetDateFilter)
                        .WhereIf(input.MaxTargetDateFilter != null, e => e.TargetDate <= input.MaxTargetDateFilter)
                        .WhereIf(input.MinActualClosureDateFilter != null, e => e.ActualClosureDate >= input.MinActualClosureDateFilter)
                        .WhereIf(input.MaxActualClosureDateFilter != null, e => e.ActualClosureDate <= input.MaxActualClosureDateFilter)
                        .WhereIf(input.MinAcceptanceDateFilter != null, e => e.AcceptanceDate >= input.MinAcceptanceDateFilter)
                        .WhereIf(input.MaxAcceptanceDateFilter != null, e => e.AcceptanceDate <= input.MaxAcceptanceDateFilter)
                        .WhereIf(input.RiskAcceptedFilter.HasValue && input.RiskAcceptedFilter > -1, e => (input.RiskAcceptedFilter == 1 && e.RiskAccepted) || (input.RiskAcceptedFilter == 0 && !e.RiskAccepted))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskTypeNameFilter), e => e.RiskTypeFk != null && e.RiskTypeFk.Name == input.RiskTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusNameFilter), e => e.StatusFk != null && e.StatusFk.Name == input.StatusNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskRatingNameFilter), e => e.RiskRatingFk != null && e.RiskRatingFk.Name == input.RiskRatingNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var pagedAndFilteredRisks = filteredRisks
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var risks = from o in pagedAndFilteredRisks
                        join o1 in _lookup_riskTypeRepository.GetAll() on o.RiskTypeId equals o1.Id into j1
                        from s1 in j1.DefaultIfEmpty()

                        join o2 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o2.Id into j2
                        from s2 in j2.DefaultIfEmpty()

                        join o3 in _lookup_statusRepository.GetAll() on o.StatusId equals o3.Id into j3
                        from s3 in j3.DefaultIfEmpty()

                        join o4 in _lookup_riskRatingRepository.GetAll() on o.RiskRatingId equals o4.Id into j4
                        from s4 in j4.DefaultIfEmpty()

                        join o5 in _lookup_userRepository.GetAll() on o.UserId equals o5.Id into j5
                        from s5 in j5.DefaultIfEmpty()

                        select new
                        {

                            o.Summary,
                            o.ExistingControl,
                            o.ERMRecommendation,
                            o.ActionPlan,
                            o.RiskOwnerComment,
                            o.TargetDate,
                            o.ActualClosureDate,
                            o.AcceptanceDate,
                            o.RiskAccepted,
                            Id = o.Id,
                            o.UserId,
                            RiskTypeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                            OrganizationUnitDisplayName = s2 == null || s2.DisplayName == null ? "" : s2.DisplayName.ToString(),
                            StatusName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                            RiskRatingName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                            UserName = s5 == null || s5.Name == null ? "" : s5.Name.ToString()
                        };

            var totalCount = await filteredRisks.CountAsync();

            var dbList = await risks.ToListAsync();
            var results = new List<GetRiskForViewDto>();

            foreach (var o in dbList)
            {
                if (o.TargetDate.ToString().IsNullOrEmpty() && o.StatusName.ToString().ToUpper() != "CLOSED")
                {
                    var res = new GetRiskForViewDto()
                    {
                        Risk = new RiskDto
                        {

                            Summary = o.Summary,
                            ExistingControl = o.ExistingControl,
                            ERMRecommendation = o.ERMRecommendation,
                            ActionPlan = o.ActionPlan,
                            RiskOwnerComment = o.RiskOwnerComment,
                            TargetDate = o.TargetDate,
                            ActualClosureDate = o.ActualClosureDate,
                            AcceptanceDate = o.AcceptanceDate,
                            RiskAccepted = o.RiskAccepted,
                            Id = o.Id,
                        },
                        RiskTypeName = o.RiskTypeName,
                        OrganizationUnitDisplayName = o.OrganizationUnitDisplayName,
                        StatusName = o.StatusName,
                        RiskRatingName = o.RiskRatingName,
                        UserName = o.UserName
                    };

                    results.Add(res);
                }
                else
                {
                    if (o.StatusName.ToString().ToUpper() != "CLOSED")
                    {
                        var res = new GetRiskForViewDto()
                        {
                            Risk = new RiskDto
                            {

                                Summary = o.Summary,
                                ExistingControl = o.ExistingControl,
                                ERMRecommendation = o.ERMRecommendation,
                                ActionPlan = o.ActionPlan,
                                RiskOwnerComment = o.RiskOwnerComment,
                                TargetDate = o.TargetDate,
                                ActualClosureDate = o.ActualClosureDate,
                                AcceptanceDate = o.AcceptanceDate,
                                RiskAccepted = o.RiskAccepted,
                                Id = o.Id,
                            },
                            RiskTypeName = o.RiskTypeName,
                            OrganizationUnitDisplayName = o.OrganizationUnitDisplayName,
                            StatusName = o.StatusName,
                            RiskRatingName = o.RiskRatingName,
                            UserName = o.UserName
                        };

                        results.Add(res);
                    }

                }


            }

            return new PagedResultDto<GetRiskForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<PagedResultDto<GetRiskForViewDto>> ClosedRisks(GetAllRisksInput input)
        {
            var dateNow = DateTime.Now;

            var filteredRisks = _riskRepository.GetAll()
                        .Include(e => e.RiskTypeFk)
                        .Include(e => e.OrganizationUnitFk)
                        .Include(e => e.StatusFk)
                        .Include(e => e.RiskRatingFk)
                        .Include(e => e.UserFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Summary.Contains(input.Filter) || e.ExistingControl.Contains(input.Filter) || e.ERMRecommendation.Contains(input.Filter) || e.ActionPlan.Contains(input.Filter) || e.RiskOwnerComment.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SummaryFilter), e => e.Summary == input.SummaryFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExistingControlFilter), e => e.ExistingControl == input.ExistingControlFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ERMRecommendationFilter), e => e.ERMRecommendation == input.ERMRecommendationFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ActionPlanFilter), e => e.ActionPlan == input.ActionPlanFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskOwnerCommentFilter), e => e.RiskOwnerComment == input.RiskOwnerCommentFilter)
                        .WhereIf(input.MinTargetDateFilter != null, e => e.TargetDate >= input.MinTargetDateFilter)
                        .WhereIf(input.MaxTargetDateFilter != null, e => e.TargetDate <= input.MaxTargetDateFilter)
                        .WhereIf(input.MinActualClosureDateFilter != null, e => e.ActualClosureDate >= input.MinActualClosureDateFilter)
                        .WhereIf(input.MaxActualClosureDateFilter != null, e => e.ActualClosureDate <= input.MaxActualClosureDateFilter)
                        .WhereIf(input.MinAcceptanceDateFilter != null, e => e.AcceptanceDate >= input.MinAcceptanceDateFilter)
                        .WhereIf(input.MaxAcceptanceDateFilter != null, e => e.AcceptanceDate <= input.MaxAcceptanceDateFilter)
                        .WhereIf(input.RiskAcceptedFilter.HasValue && input.RiskAcceptedFilter > -1, e => (input.RiskAcceptedFilter == 1 && e.RiskAccepted) || (input.RiskAcceptedFilter == 0 && !e.RiskAccepted))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskTypeNameFilter), e => e.RiskTypeFk != null && e.RiskTypeFk.Name == input.RiskTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusNameFilter), e => e.StatusFk != null && e.StatusFk.Name == input.StatusNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskRatingNameFilter), e => e.RiskRatingFk != null && e.RiskRatingFk.Name == input.RiskRatingNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var pagedAndFilteredRisks = filteredRisks
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var risks = from o in pagedAndFilteredRisks
                        join o1 in _lookup_riskTypeRepository.GetAll() on o.RiskTypeId equals o1.Id into j1
                        from s1 in j1.DefaultIfEmpty()

                        join o2 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o2.Id into j2
                        from s2 in j2.DefaultIfEmpty()

                        join o3 in _lookup_statusRepository.GetAll() on o.StatusId equals o3.Id into j3
                        from s3 in j3.DefaultIfEmpty()

                        join o4 in _lookup_riskRatingRepository.GetAll() on o.RiskRatingId equals o4.Id into j4
                        from s4 in j4.DefaultIfEmpty()

                        join o5 in _lookup_userRepository.GetAll() on o.UserId equals o5.Id into j5
                        from s5 in j5.DefaultIfEmpty()

                        select new
                        {

                            o.Summary,
                            o.ExistingControl,
                            o.ERMRecommendation,
                            o.ActionPlan,
                            o.RiskOwnerComment,
                            o.TargetDate,
                            o.ActualClosureDate,
                            o.AcceptanceDate,
                            o.RiskAccepted,
                            Id = o.Id,
                            o.UserId,
                            RiskTypeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                            OrganizationUnitDisplayName = s2 == null || s2.DisplayName == null ? "" : s2.DisplayName.ToString(),
                            StatusName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                            RiskRatingName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                            UserName = s5 == null || s5.Name == null ? "" : s5.Name.ToString()
                        };

            var totalCount = await filteredRisks.CountAsync();

            var dbList = await risks.ToListAsync();
            var results = new List<GetRiskForViewDto>();

            foreach (var o in dbList)
            {
                if (o.StatusName.ToUpper() == "CLOSED")
                {
                    var res = new GetRiskForViewDto()
                    {
                        Risk = new RiskDto
                        {

                            Summary = o.Summary,
                            ExistingControl = o.ExistingControl,
                            ERMRecommendation = o.ERMRecommendation,
                            ActionPlan = o.ActionPlan,
                            RiskOwnerComment = o.RiskOwnerComment,
                            TargetDate = o.TargetDate,
                            ActualClosureDate = o.ActualClosureDate,
                            AcceptanceDate = o.AcceptanceDate,
                            RiskAccepted = o.RiskAccepted,
                            Id = o.Id,
                        },
                        RiskTypeName = o.RiskTypeName,
                        OrganizationUnitDisplayName = o.OrganizationUnitDisplayName,
                        StatusName = o.StatusName,
                        RiskRatingName = o.RiskRatingName,
                        UserName = o.UserName
                    };

                    results.Add(res);
                }


            }

            return new PagedResultDto<GetRiskForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<PagedResultDto<GetRiskForViewDto>> FilteredRisks(GetAllRisksInput input)
        {
            var dateNow = DateTime.Now;

            var filteredRisks = _riskRepository.GetAll()
                        .Include(e => e.RiskTypeFk)
                        .Include(e => e.OrganizationUnitFk)
                        .Include(e => e.StatusFk)
                        .Include(e => e.RiskRatingFk)
                        .Include(e => e.UserFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Summary.Contains(input.Filter) || e.ExistingControl.Contains(input.Filter) || e.ERMRecommendation.Contains(input.Filter) || e.ActionPlan.Contains(input.Filter) || e.RiskOwnerComment.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SummaryFilter), e => e.Summary == input.SummaryFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExistingControlFilter), e => e.ExistingControl == input.ExistingControlFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ERMRecommendationFilter), e => e.ERMRecommendation == input.ERMRecommendationFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ActionPlanFilter), e => e.ActionPlan == input.ActionPlanFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskOwnerCommentFilter), e => e.RiskOwnerComment == input.RiskOwnerCommentFilter)
                        .WhereIf(input.MinTargetDateFilter != null, e => e.TargetDate >= input.MinTargetDateFilter)
                        .WhereIf(input.MaxTargetDateFilter != null, e => e.TargetDate <= input.MaxTargetDateFilter)
                        .WhereIf(input.MinActualClosureDateFilter != null, e => e.ActualClosureDate >= input.MinActualClosureDateFilter)
                        .WhereIf(input.MaxActualClosureDateFilter != null, e => e.ActualClosureDate <= input.MaxActualClosureDateFilter)
                        .WhereIf(input.MinAcceptanceDateFilter != null, e => e.AcceptanceDate >= input.MinAcceptanceDateFilter)
                        .WhereIf(input.MaxAcceptanceDateFilter != null, e => e.AcceptanceDate <= input.MaxAcceptanceDateFilter)
                        .WhereIf(input.RiskAcceptedFilter.HasValue && input.RiskAcceptedFilter > -1, e => (input.RiskAcceptedFilter == 1 && e.RiskAccepted) || (input.RiskAcceptedFilter == 0 && !e.RiskAccepted))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskTypeNameFilter), e => e.RiskTypeFk != null && e.RiskTypeFk.Name == input.RiskTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusNameFilter), e => e.StatusFk != null && e.StatusFk.Name == input.StatusNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskRatingNameFilter), e => e.RiskRatingFk != null && e.RiskRatingFk.Name == input.RiskRatingNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var pagedAndFilteredRisks = filteredRisks
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var risks = from o in pagedAndFilteredRisks
                        join o1 in _lookup_riskTypeRepository.GetAll() on o.RiskTypeId equals o1.Id into j1
                        from s1 in j1.DefaultIfEmpty()

                        join o2 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o2.Id into j2
                        from s2 in j2.DefaultIfEmpty()

                        join o3 in _lookup_statusRepository.GetAll() on o.StatusId equals o3.Id into j3
                        from s3 in j3.DefaultIfEmpty()

                        join o4 in _lookup_riskRatingRepository.GetAll() on o.RiskRatingId equals o4.Id into j4
                        from s4 in j4.DefaultIfEmpty()

                        join o5 in _lookup_userRepository.GetAll() on o.UserId equals o5.Id into j5
                        from s5 in j5.DefaultIfEmpty()

                        select new
                        {

                            o.Summary,
                            o.ExistingControl,
                            o.ERMRecommendation,
                            o.ActionPlan,
                            o.RiskOwnerComment,
                            o.TargetDate,
                            o.ActualClosureDate,
                            o.AcceptanceDate,
                            o.RiskAccepted,
                            Id = o.Id,
                            o.UserId,
                            RiskTypeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                            OrganizationUnitDisplayName = s2 == null || s2.DisplayName == null ? "" : s2.DisplayName.ToString(),
                            StatusName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                            RiskRatingName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                            UserName = s5 == null || s5.Name == null ? "" : s5.Name.ToString()
                        };

            var totalCount = await filteredRisks.CountAsync();

            var dbList = await risks.ToListAsync();
            var results = new List<GetRiskForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetRiskForViewDto()
                {
                    Risk = new RiskDto
                    {

                        Summary = o.Summary,
                        ExistingControl = o.ExistingControl,
                        ERMRecommendation = o.ERMRecommendation,
                        ActionPlan = o.ActionPlan,
                        RiskOwnerComment = o.RiskOwnerComment,
                        TargetDate = o.TargetDate,
                        ActualClosureDate = o.ActualClosureDate,
                        AcceptanceDate = o.AcceptanceDate,
                        RiskAccepted = o.RiskAccepted,
                        Id = o.Id,
                    },
                    RiskTypeName = o.RiskTypeName,
                    OrganizationUnitDisplayName = o.OrganizationUnitDisplayName,
                    StatusName = o.StatusName,
                    RiskRatingName = o.RiskRatingName,
                    UserName = o.UserName
                };

                results.Add(res);

            }

            return new PagedResultDto<GetRiskForViewDto>(
                totalCount,
                results
            );

        }

        //public void SendEmailUsingGmail(SendEmailNotificationDTO email)
        //{
        //    string fromMail = "youngsolomon072@gmail.com";
        //    string fromPassword = "byjbzjzccoktssgu";
        //    MailMessage message = new MailMessage();
        //    message.From = new MailAddress(fromMail);
        //    message.Subject = "Test Subject";
        //    foreach (var item in email.Recipients)
        //    {
        //        message.To.Add(new MailAddress(item));
        //    }
        //    message.Body = email.HtmlContent;
        //    message.IsBodyHtml = true;
        //    var smtpClient = new SmtpClient("smtp.gmail.com")
        //    {
        //        Port = 587,
        //        Credentials = new NetworkCredential(fromMail, fromPassword),
        //        EnableSsl = true,
        //    };
        //    smtpClient.Send(message);
        //}

        public async Task SendRiskEmail(SendRisksEmailInput input)
        {
            try
            {
                await _emailSender.SendAsync(
                    input.EmailAddress,
                    input.Subject,
                    input.Body
                );
            }
            catch (Exception e)
            {
                throw new UserFriendlyException("An error was encountered while sending an email. " + e.Message, e);
            }
        }

        private StringBuilder GetTitleAndSubTitle(int? tenantId, string title, string subTitle)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(tenantId));
            emailTemplate.Replace("{EMAIL_TITLE}", title);
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", subTitle);

            return emailTemplate;
        }

        private async Task ReplaceBodyAndSend(string emailAddress, string subject, StringBuilder emailTemplate,
            StringBuilder mailMessage)
        {
            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());
            await _emailSender.SendAsync(new MailMessage
            {
                To = { emailAddress },
                Subject = subject,
                Body = emailTemplate.ToString(),
                IsBodyHtml = true
            });
        }

        public int GetERMId(int id)
        {
            //var eRMUserId = _riskRepository.Get(id).CreatorUserId;
           
            return (int)_riskRepository.Get(id).CreatorUserId;
        }

        public string GetERMEmail(int id)
        {
            var ermUserId = (int)_riskRepository.Get(id).CreatorUserId;
            var email = _userManager.Users.Where(u=>u.Id == ermUserId).FirstOrDefault().EmailAddress;

            return email;
        }
    }

}