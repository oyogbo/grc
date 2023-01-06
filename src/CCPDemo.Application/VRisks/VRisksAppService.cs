using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.VRisks.Exporting;
using CCPDemo.VRisks.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;
using Abp.Net.Mail;
using Abp.Net.Mail.Smtp;
using System.Net.Mail;
using System.Threading;
using CCPDemo.Authorization.Users;

namespace CCPDemo.VRisks
{
    [AbpAuthorize(AppPermissions.Pages_VRisks)]
    public class VRisksAppService : CCPDemoAppServiceBase, IVRisksAppService
    {
        private readonly IRepository<VRisk> _vRiskRepository;
        private readonly IVRisksExcelExporter _vRisksExcelExporter;
        private readonly IEmailSender _emailSender;
        private readonly UserManager _userManager;

        private readonly ISmtpEmailSenderConfiguration _smtpConfig;

        public VRisksAppService(IRepository<VRisk> vRiskRepository, IVRisksExcelExporter vRisksExcelExporter, 
            IEmailSender emailSender, ISmtpEmailSenderConfiguration smtpConfig, UserManager userManager)
        {
            _vRiskRepository = vRiskRepository;
            _vRisksExcelExporter = vRisksExcelExporter;
            _emailSender = emailSender;
            _smtpConfig = smtpConfig;
            _userManager = userManager;
        }

        public async Task<PagedResultDto<GetVRiskForViewDto>> GetAll(GetAllVRisksInput input)
        {

            var filteredVRisks = _vRiskRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Department.Contains(input.Filter) || e.RiskOwner.Contains(input.Filter) || e.ResolutionTimeLine.Contains(input.Filter) || e.ERMComment.Contains(input.Filter) || e.RiskOwnerComment.Contains(input.Filter) || e.Status.Contains(input.Filter) || e.ActualClosureDate.Contains(input.Filter) || e.MitigationDate.Contains(input.Filter) || e.AcceptanceDate.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DepartmentFilter), e => e.Department == input.DepartmentFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskOwnerFilter), e => e.RiskOwner == input.RiskOwnerFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ResolutionTimeLineFilter), e => e.ResolutionTimeLine == input.ResolutionTimeLineFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ERMCommentFilter), e => e.ERMComment == input.ERMCommentFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskOwnerCommentFilter), e => e.RiskOwnerComment == input.RiskOwnerCommentFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusFilter), e => e.Status == input.StatusFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ActualClosureDateFilter), e => e.ActualClosureDate == input.ActualClosureDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MitigationDateFilter), e => e.MitigationDate == input.MitigationDateFilter)
                        .WhereIf(input.AcceptRiskFilter.HasValue && input.AcceptRiskFilter > -1, e => (input.AcceptRiskFilter == 1 && e.AcceptRisk) || (input.AcceptRiskFilter == 0 && !e.AcceptRisk))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AcceptanceDateFilter), e => e.AcceptanceDate == input.AcceptanceDateFilter);

            var pagedAndFilteredVRisks = filteredVRisks
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var vRisks = from o in pagedAndFilteredVRisks
                         select new
                         {

                             o.Name,
                             o.Description,
                             o.Department,
                             o.RiskOwner,
                             o.ResolutionTimeLine,
                             o.ERMComment,
                             o.RiskOwnerComment,
                             o.Status,
                             o.ActualClosureDate,
                             o.MitigationDate,
                             o.AcceptRisk,
                             o.AcceptanceDate,
                             Id = o.Id
                         };

            var totalCount = await filteredVRisks.CountAsync();

            var dbList = await vRisks.ToListAsync();
            var results = new List<GetVRiskForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetVRiskForViewDto()
                {
                    VRisk = new VRiskDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Department = o.Department,
                        RiskOwner = o.RiskOwner,
                        ResolutionTimeLine = o.ResolutionTimeLine,
                        ERMComment = o.ERMComment,
                        RiskOwnerComment = o.RiskOwnerComment,
                        Status = o.Status,
                        ActualClosureDate = o.ActualClosureDate,
                        MitigationDate = o.MitigationDate,
                        AcceptRisk = o.AcceptRisk,
                        AcceptanceDate = o.AcceptanceDate,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetVRiskForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetVRiskForViewDto> GetVRiskForView(int id)
        {
            var vRisk = await _vRiskRepository.GetAsync(id);

            var output = new GetVRiskForViewDto { VRisk = ObjectMapper.Map<VRiskDto>(vRisk) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_VRisks_Edit)]
        public async Task<GetVRiskForEditOutput> GetVRiskForEdit(EntityDto input)
        {
            var vRisk = await _vRiskRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetVRiskForEditOutput { VRisk = ObjectMapper.Map<CreateOrEditVRiskDto>(vRisk) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditVRiskDto input)
        {
            //input.AcceptRisk = true;
            
            if (input.Id == null)
            {
                await Create(input);
                //await ExecuteSendMail();
            }
            else
            {
                await Update(input);
                //await ExecuteSendMail();
            }
        }

        [AbpAuthorize(AppPermissions.Pages_VRisks_Create)]
        protected virtual async Task Create(CreateOrEditVRiskDto input)
        {
            //input.AcceptRisk = true;
            var vRisk = ObjectMapper.Map<VRisk>(input);

            if (AbpSession.TenantId != null)
            {
                vRisk.TenantId = (int?)AbpSession.TenantId;
            }

            await _vRiskRepository.InsertAsync(vRisk);

            //var riskOwner = _userManager.Users.Where(u => u.FullName == input.RiskOwner).FirstOrDefault();
            //var userId = riskOwner.Id;
            //CreateOrEditUserRiskMitigationDto dto = new CreateOrEditUserRiskMitigationDto();
            //dto.Name = input.Name;
            //dto.Description = input.Description;
            //dto.Department = input.Department;
            //dto.ResolutionTimeLine = input.ResolutionTimeLine;
            //dto.ERMComment = input.ERMComment;
            //dto.RiskOwnerComment = input.RiskOwnerComment;
            //dto.Status = input.Status;
            //dto.Rating = input.Rating;
            //dto.UserId = userId;
            //dto.MitigationDate = input.MitigationDate;
            //dto.AcceptRisk = input.AcceptRisk;
            //dto.AcceptanceDate = input.AcceptanceDate;

            //await _userRiskMitigationsAppService.CreateOrEdit(dto);

            //var riskMitigation = ObjectMapper.Map<UserRiskMitigation>(dto);
            //await _userRiskMitigationRepository.InsertAsync(riskMitigation);

            // await _vRiskRepository.InsertAsync(vRisk);

        }

        [AbpAuthorize(AppPermissions.Pages_VRisks_Edit)]
        protected virtual async Task Update(CreateOrEditVRiskDto input)
        {
            //input.AcceptRisk = true;
            var vRisk = await _vRiskRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, vRisk);

        }

        [AbpAuthorize(AppPermissions.Pages_VRisks_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _vRiskRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetVRisksToExcel(GetAllVRisksForExcelInput input)
        {

            var filteredVRisks = _vRiskRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Department.Contains(input.Filter) || e.RiskOwner.Contains(input.Filter) || e.ResolutionTimeLine.Contains(input.Filter) || e.ERMComment.Contains(input.Filter) || e.RiskOwnerComment.Contains(input.Filter) || e.Status.Contains(input.Filter) || e.ActualClosureDate.Contains(input.Filter) || e.MitigationDate.Contains(input.Filter) || e.AcceptanceDate.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DepartmentFilter), e => e.Department == input.DepartmentFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskOwnerFilter), e => e.RiskOwner == input.RiskOwnerFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ResolutionTimeLineFilter), e => e.ResolutionTimeLine == input.ResolutionTimeLineFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ERMCommentFilter), e => e.ERMComment == input.ERMCommentFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskOwnerCommentFilter), e => e.RiskOwnerComment == input.RiskOwnerCommentFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusFilter), e => e.Status == input.StatusFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ActualClosureDateFilter), e => e.ActualClosureDate == input.ActualClosureDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MitigationDateFilter), e => e.MitigationDate == input.MitigationDateFilter)
                        .WhereIf(input.AcceptRiskFilter.HasValue && input.AcceptRiskFilter > -1, e => (input.AcceptRiskFilter == 1 && e.AcceptRisk) || (input.AcceptRiskFilter == 0 && !e.AcceptRisk))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AcceptanceDateFilter), e => e.AcceptanceDate == input.AcceptanceDateFilter);

            var query = (from o in filteredVRisks
                         select new GetVRiskForViewDto()
                         {
                             VRisk = new VRiskDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Department = o.Department,
                                 RiskOwner = o.RiskOwner,
                                 ResolutionTimeLine = o.ResolutionTimeLine,
                                 ERMComment = o.ERMComment,
                                 RiskOwnerComment = o.RiskOwnerComment,
                                 Status = o.Status,
                                 ActualClosureDate = o.ActualClosureDate,
                                 MitigationDate = o.MitigationDate,
                                 AcceptRisk = o.AcceptRisk,
                                 AcceptanceDate = o.AcceptanceDate,
                                 Id = o.Id
                             }
                         });

            var vRiskListDtos = await query.ToListAsync();

            return _vRisksExcelExporter.ExportToFile(vRiskListDtos);
        }

        public async Task SendEmail()
        {
            await _emailSender.SendAsync(
                to: "oyogbo@gmail.com",
                subject: "You have a new task!",
                body: $"A new task is assigned for you: <b>Risky</b>",
                isBodyHtml: true
            );
            
        }

        public async Task ExecuteSendMail()
        {
            CancellationToken token = new CancellationToken();
            var smtpClient = new SmtpEmailSender(_smtpConfig).BuildClient();

            var from = new MailAddress(_smtpConfig.UserName);

            await smtpClient.SendMailAsync(new MailMessage(from, new MailAddress("wilf.funsho@gmail.com"))
            {
                Subject = "Risk Raised",
                Body = "RISK",
                IsBodyHtml = true
            },token);
        }

        public async Task InsertUserRiskmitigation()
        {

        }
    }


}