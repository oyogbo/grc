using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.KeyRiskIndicators.Exporting;
using CCPDemo.KeyRiskIndicators.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;
using System.IO;
using CsvHelper;
using System.Globalization;
using IdentityServer4.Test;
using PayPalCheckoutSdk.Orders;

namespace CCPDemo.KeyRiskIndicators
{
    [AbpAuthorize(AppPermissions.Pages_KeyRiskIndicators)]
    public class KeyRiskIndicatorsAppService : CCPDemoAppServiceBase, IKeyRiskIndicatorsAppService
    {
        private readonly IRepository<KeyRiskIndicator> _keyRiskIndicatorRepository;
        private readonly IKeyRiskIndicatorsExcelExporter _keyRiskIndicatorsExcelExporter;

        public KeyRiskIndicatorsAppService(IRepository<KeyRiskIndicator> keyRiskIndicatorRepository, IKeyRiskIndicatorsExcelExporter keyRiskIndicatorsExcelExporter)
        {
            _keyRiskIndicatorRepository = keyRiskIndicatorRepository;
            _keyRiskIndicatorsExcelExporter = keyRiskIndicatorsExcelExporter;

        }

        public async Task<PagedResultDto<GetKeyRiskIndicatorForViewDto>> GetAll(GetAllKeyRiskIndicatorsInput input)
        {

            var filteredKeyRiskIndicators = _keyRiskIndicatorRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ReferenceId.Contains(input.Filter) || e.BusinessLines.Contains(input.Filter) || e.Activity.Contains(input.Filter) || e.Process.Contains(input.Filter) || e.SubProcess.Contains(input.Filter) || e.PotentialRisk.Contains(input.Filter) || e.LikelihoodOfOccurrence_irr.Contains(input.Filter) || e.LikelihoodOfImpact_irr.Contains(input.Filter) || e.KeyControl.Contains(input.Filter) || e.ControlEffectiveness.Contains(input.Filter) || e.LikelihoodOfOccurrence_rrr.Contains(input.Filter) || e.LikelihoodOfImpact_rrr.Contains(input.Filter) || e.MitigationPlan.Contains(input.Filter) || e.Comment.Contains(input.Filter) || e.Status.Contains(input.Filter) || e.OwnerComment.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceIdFilter), e => e.ReferenceId == input.ReferenceIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessLinesFilter), e => e.BusinessLines == input.BusinessLinesFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ActivityFilter), e => e.Activity == input.ActivityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessFilter), e => e.Process == input.ProcessFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubProcessFilter), e => e.SubProcess == input.SubProcessFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PotentialRiskFilter), e => e.PotentialRisk == input.PotentialRiskFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LikelihoodOfOccurrence_irrFilter), e => e.LikelihoodOfOccurrence_irr == input.LikelihoodOfOccurrence_irrFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LikelihoodOfImpact_irrFilter), e => e.LikelihoodOfImpact_irr == input.LikelihoodOfImpact_irrFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.KeyControlFilter), e => e.KeyControl == input.KeyControlFilter)
                        .WhereIf(input.IsControlInUseFilter.HasValue && input.IsControlInUseFilter > -1, e => (input.IsControlInUseFilter == 1 && e.IsControlInUse) || (input.IsControlInUseFilter == 0 && !e.IsControlInUse))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ControlEffectivenessFilter), e => e.ControlEffectiveness == input.ControlEffectivenessFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LikelihoodOfOccurrence_rrrFilter), e => e.LikelihoodOfOccurrence_rrr == input.LikelihoodOfOccurrence_rrrFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LikelihoodOfImpact_rrrFilter), e => e.LikelihoodOfImpact_rrr == input.LikelihoodOfImpact_rrrFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MitigationPlanFilter), e => e.MitigationPlan == input.MitigationPlanFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CommentFilter), e => e.Comment == input.CommentFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusFilter), e => e.Status == input.StatusFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OwnerCommentFilter), e => e.OwnerComment == input.OwnerCommentFilter);

            var pagedAndFilteredKeyRiskIndicators = filteredKeyRiskIndicators
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var keyRiskIndicators = from o in pagedAndFilteredKeyRiskIndicators
                                    select new
                                    {
                                        o.ReferenceId,
                                        o.BusinessLines,
                                        o.Activity,
                                        o.Process,
                                        o.SubProcess,
                                        o.PotentialRisk,
                                        o.LikelihoodOfOccurrence_irr,
                                        o.LikelihoodOfImpact_irr,
                                        o.KeyControl,
                                        o.IsControlInUse,
                                        o.ControlEffectiveness,
                                        o.LikelihoodOfOccurrence_rrr,
                                        o.LikelihoodOfImpact_rrr,
                                        o.MitigationPlan,
                                        o.Comment,
                                        o.Status,
                                        o.OwnerComment,
                                        Id = o.Id
                                    };

            var totalCount = await filteredKeyRiskIndicators.CountAsync();

            var dbList = await keyRiskIndicators.ToListAsync();
            var results = new List<GetKeyRiskIndicatorForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetKeyRiskIndicatorForViewDto()
                {
                    KeyRiskIndicator = new KeyRiskIndicatorDto
                    {
                        ReferenceId = o.ReferenceId,
                        BusinessLines = o.BusinessLines,
                        Activity = o.Activity,
                        Process = o.Process,
                        SubProcess = o.SubProcess,
                        PotentialRisk = o.PotentialRisk,
                        LikelihoodOfOccurrence_irr = o.LikelihoodOfOccurrence_irr,
                        LikelihoodOfImpact_irr = o.LikelihoodOfImpact_irr,
                        KeyControl = o.KeyControl,
                        IsControlInUse = o.IsControlInUse,
                        ControlEffectiveness = o.ControlEffectiveness,
                        LikelihoodOfOccurrence_rrr = o.LikelihoodOfOccurrence_rrr,
                        LikelihoodOfImpact_rrr = o.LikelihoodOfImpact_rrr,
                        MitigationPlan = o.MitigationPlan,
                        Comment = o.Comment,
                        Status = o.Status,
                        OwnerComment = o.OwnerComment,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetKeyRiskIndicatorForViewDto>(
                totalCount,
                results
            );

        }


        public async Task<GetKeyRiskIndicatorForViewDto> GetKeyRiskIndicatorForView(int id)
        {
            var keyRiskIndicator = await _keyRiskIndicatorRepository.GetAsync(id);
            var output = new GetKeyRiskIndicatorForViewDto { KeyRiskIndicator = ObjectMapper.Map<KeyRiskIndicatorDto>(keyRiskIndicator) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_KeyRiskIndicators_Edit)]
        public async Task<GetKeyRiskIndicatorForEditOutput> GetKeyRiskIndicatorForEdit(EntityDto input)
        {
            var keyRiskIndicator = await _keyRiskIndicatorRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetKeyRiskIndicatorForEditOutput { KeyRiskIndicator = ObjectMapper.Map<CreateOrEditKeyRiskIndicatorDto>(keyRiskIndicator) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditKeyRiskIndicatorDto input)
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

        [AbpAuthorize(AppPermissions.Pages_KeyRiskIndicators_Create)]
        protected virtual async Task Create(CreateOrEditKeyRiskIndicatorDto input)
        {

            KeyRiskIndicator dataToAdd = new KeyRiskIndicator();


            dataToAdd.ReferenceId = input.ReferenceId;
            dataToAdd.Status = "Not Approved";
            dataToAdd.Activity = input.Activity;
            dataToAdd.BusinessLines = input.BusinessLines;
            dataToAdd.PotentialRisk = input.PotentialRisk;
            dataToAdd.LikelihoodOfOccurrence_rrr = input.LikelihoodOfOccurrence_rrr;
            dataToAdd.LikelihoodOfImpact_rrr = input.LikelihoodOfImpact_rrr;
            dataToAdd.LikelihoodOfImpact_irr = input.LikelihoodOfImpact_irr;
            dataToAdd.LikelihoodOfOccurrence_irr = input.LikelihoodOfOccurrence_irr;
            dataToAdd.MitigationPlan = input.MitigationPlan;
            dataToAdd.SubProcess = input.SubProcess;
            dataToAdd.Process = input.Process;
            dataToAdd.ControlEffectiveness = input.ControlEffectiveness;
            dataToAdd.IsControlInUse = input.IsControlInUse;
            dataToAdd.KeyControl = input.KeyControl;
          

           // var keyRiskIndicator = ObjectMapper.Map<KeyRiskIndicator>(input);

            await _keyRiskIndicatorRepository.InsertAsync(dataToAdd);

        }

        




        [AbpAuthorize(AppPermissions.Pages_KeyRiskIndicators_Edit)]
        protected virtual async Task Update(CreateOrEditKeyRiskIndicatorDto input)
        {
            var keyRiskIndicator = await _keyRiskIndicatorRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, keyRiskIndicator);
        }



        [AbpAuthorize(AppPermissions.Pages_KeyRiskIndicators_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _keyRiskIndicatorRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetKeyRiskIndicatorsToExcel(GetAllKeyRiskIndicatorsForExcelInput input)
        {

            var filteredKeyRiskIndicators = _keyRiskIndicatorRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ReferenceId.Contains(input.Filter) || e.BusinessLines.Contains(input.Filter) || e.Activity.Contains(input.Filter) || e.Process.Contains(input.Filter) || e.SubProcess.Contains(input.Filter) || e.PotentialRisk.Contains(input.Filter) || e.LikelihoodOfOccurrence_irr.Contains(input.Filter) || e.LikelihoodOfImpact_irr.Contains(input.Filter) || e.KeyControl.Contains(input.Filter) || e.ControlEffectiveness.Contains(input.Filter) || e.LikelihoodOfOccurrence_rrr.Contains(input.Filter) || e.LikelihoodOfImpact_rrr.Contains(input.Filter) || e.MitigationPlan.Contains(input.Filter) || e.Comment.Contains(input.Filter) || e.Status.Contains(input.Filter) || e.OwnerComment.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceIdFilter), e => e.ReferenceId == input.ReferenceIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessLinesFilter), e => e.BusinessLines == input.BusinessLinesFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ActivityFilter), e => e.Activity == input.ActivityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessFilter), e => e.Process == input.ProcessFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubProcessFilter), e => e.SubProcess == input.SubProcessFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PotentialRiskFilter), e => e.PotentialRisk == input.PotentialRiskFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LikelihoodOfOccurrence_irrFilter), e => e.LikelihoodOfOccurrence_irr == input.LikelihoodOfOccurrence_irrFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LikelihoodOfImpact_irrFilter), e => e.LikelihoodOfImpact_irr == input.LikelihoodOfImpact_irrFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.KeyControlFilter), e => e.KeyControl == input.KeyControlFilter)
                        .WhereIf(input.IsControlInUseFilter.HasValue && input.IsControlInUseFilter > -1, e => (input.IsControlInUseFilter == 1 && e.IsControlInUse) || (input.IsControlInUseFilter == 0 && !e.IsControlInUse))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ControlEffectivenessFilter), e => e.ControlEffectiveness == input.ControlEffectivenessFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LikelihoodOfOccurrence_rrrFilter), e => e.LikelihoodOfOccurrence_rrr == input.LikelihoodOfOccurrence_rrrFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LikelihoodOfImpact_rrrFilter), e => e.LikelihoodOfImpact_rrr == input.LikelihoodOfImpact_rrrFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MitigationPlanFilter), e => e.MitigationPlan == input.MitigationPlanFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CommentFilter), e => e.Comment == input.CommentFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusFilter), e => e.Status == input.StatusFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OwnerCommentFilter), e => e.OwnerComment == input.OwnerCommentFilter);

            var query = (from o in filteredKeyRiskIndicators
                         select new GetKeyRiskIndicatorForViewDto()
                         {
                             KeyRiskIndicator = new KeyRiskIndicatorDto
                             {
                                 BusinessLines = o.BusinessLines,
                                 Activity = o.Activity,
                                 Process = o.Process,
                                 SubProcess = o.SubProcess,
                                 PotentialRisk = o.PotentialRisk,
                                 LikelihoodOfOccurrence_irr = o.LikelihoodOfOccurrence_irr,
                                 LikelihoodOfImpact_irr = o.LikelihoodOfImpact_irr,
                                 KeyControl = o.KeyControl,
                                 IsControlInUse = o.IsControlInUse,
                                 ControlEffectiveness = o.ControlEffectiveness,
                                 LikelihoodOfOccurrence_rrr = o.LikelihoodOfOccurrence_rrr,
                                 LikelihoodOfImpact_rrr = o.LikelihoodOfImpact_rrr,
                                 MitigationPlan = o.MitigationPlan,
                                 Comment = o.Comment,
                                 Status = o.Status,
                                 OwnerComment = o.OwnerComment,
                                 Id = o.Id
                             }
                         });

            var keyRiskIndicatorListDtos = await query.ToListAsync();

            return _keyRiskIndicatorsExcelExporter.ExportToFile(keyRiskIndicatorListDtos);
        }

        IEnumerable<T> IKeyRiskIndicatorsAppService.ReadCSV<T>(Stream file)
        {
            var reader = new StreamReader(file);
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<T>();
            return records;
        }

        void IKeyRiskIndicatorsAppService.WriteCSV<T>(List<T> records)
        {
            using (var writer = new StreamWriter("C:\\file.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(records);
            }
        }

        public async Task<bool> DeclineKRI(int Id)
        {
            var KRIFromDb = _keyRiskIndicatorRepository.Get(Id);
            if (KRIFromDb == null)
                return false;
            KRIFromDb.Status = "Rejected";
            _keyRiskIndicatorRepository.Update(KRIFromDb);
            return true;
        }

        public async Task<bool> ApproveclineKRI(List<int> Ids)
        {

            foreach (var id in Ids)
            {
                var KRIFromDb = _keyRiskIndicatorRepository.Get(id);
                if (KRIFromDb != null)
                {
                    KRIFromDb.Status = "Approved";
                    _keyRiskIndicatorRepository.Update(KRIFromDb);
                }
            }
            return true;
        }

        public async Task<GetKeyRiskIndicatorForViewDto> GetAllByRefId(string RefId)
        {
            GetKeyRiskIndicatorForViewDto dataToReturn = new GetKeyRiskIndicatorForViewDto();
            List<KeyRiskIndicator> itemsFromDb = _keyRiskIndicatorRepository
                                                                    .GetAll()
                                                                    .Where(x => x.ReferenceId == RefId)
                                                                    .ToList();
            foreach (var item in itemsFromDb)
            {
                KeyRiskIndicatorDto keyRiskIndicatorDto = new KeyRiskIndicatorDto();

                keyRiskIndicatorDto.Activity = item.Activity;
                keyRiskIndicatorDto.Status = item.Status;
                keyRiskIndicatorDto.BusinessLines = item.BusinessLines;
                keyRiskIndicatorDto.PotentialRisk = item.PotentialRisk;
                keyRiskIndicatorDto.MitigationPlan = item.MitigationPlan;
                keyRiskIndicatorDto.OwnerComment = item.OwnerComment;
                keyRiskIndicatorDto.PotentialRisk = item.PotentialRisk;
                keyRiskIndicatorDto.ControlEffectiveness = item.ControlEffectiveness;
                keyRiskIndicatorDto.SubProcess = item.SubProcess;
                keyRiskIndicatorDto.Process = item.Process;
                keyRiskIndicatorDto.KeyControl = item.KeyControl;
                keyRiskIndicatorDto.LikelihoodOfImpact_irr = item.LikelihoodOfImpact_irr;
                keyRiskIndicatorDto.LikelihoodOfImpact_rrr = item.LikelihoodOfImpact_rrr;
                keyRiskIndicatorDto.LikelihoodOfOccurrence_irr = item.LikelihoodOfOccurrence_irr;
                keyRiskIndicatorDto.LikelihoodOfOccurrence_rrr = item.LikelihoodOfOccurrence_rrr;
                keyRiskIndicatorDto.IsControlInUse = item.IsControlInUse;
                keyRiskIndicatorDto.Id = item.Id;
                keyRiskIndicatorDto.ReferenceId = item.ReferenceId;

                dataToReturn.KeyRiskIndicators.Add(keyRiskIndicatorDto);
            }
            return dataToReturn;
        }

        public async Task<List<string>> GetERMEmails()
        {
            var users = await GetAllErm();
            List<string> emailsToReturn = new List<string>();

            if (users.Count() > 0)
            {
                foreach (var item in users)
                {
                    emailsToReturn.Add(item.EmailAddress);
                }

                return emailsToReturn;
            }
            return emailsToReturn;

        }
    }
}