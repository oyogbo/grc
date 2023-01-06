using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.RiskTransactions.Exporting;
using CCPDemo.RiskTransactions.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;

namespace CCPDemo.RiskTransactions
{
    public class RiskTransactionsAppService : CCPDemoAppServiceBase, IRiskTransactionsAppService
    {
        private readonly IRepository<RiskTransaction> _riskTransactionRepository;
        private readonly IRiskTransactionsExcelExporter _riskTransactionsExcelExporter;

        public RiskTransactionsAppService(IRepository<RiskTransaction> riskTransactionRepository, IRiskTransactionsExcelExporter riskTransactionsExcelExporter)
        {
            _riskTransactionRepository = riskTransactionRepository;
            _riskTransactionsExcelExporter = riskTransactionsExcelExporter;

        }

        public async Task<PagedResultDto<GetRiskTransactionForViewDto>> GetAll(GetAllRiskTransactionsInput input)
        {

            var filteredRiskTransactions = _riskTransactionRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TransactionType.Contains(input.Filter) || e.Date.Contains(input.Filter) || e.CurrentValue.Contains(input.Filter) || e.NewValue.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionTypeFilter), e => e.TransactionType == input.TransactionTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DateFilter), e => e.Date == input.DateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrentValueFilter), e => e.CurrentValue == input.CurrentValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NewValueFilter), e => e.NewValue == input.NewValueFilter)
                        .WhereIf(input.MinRiskIdFilter != null, e => e.RiskId >= input.MinRiskIdFilter)
                        .WhereIf(input.MaxRiskIdFilter != null, e => e.RiskId <= input.MaxRiskIdFilter)
                        .WhereIf(input.MinUserIdFilter != null, e => e.UserId >= input.MinUserIdFilter)
                        .WhereIf(input.MaxUserIdFilter != null, e => e.UserId <= input.MaxUserIdFilter);

            var pagedAndFilteredRiskTransactions = filteredRiskTransactions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var riskTransactions = from o in pagedAndFilteredRiskTransactions
                                   select new
                                   {

                                       o.TransactionType,
                                       o.Date,
                                       o.CurrentValue,
                                       o.NewValue,
                                       o.RiskId,
                                       o.UserId,
                                       Id = o.Id
                                   };

            var totalCount = await filteredRiskTransactions.CountAsync();

            var dbList = await riskTransactions.ToListAsync();
            var results = new List<GetRiskTransactionForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetRiskTransactionForViewDto()
                {
                    RiskTransaction = new RiskTransactionDto
                    {

                        TransactionType = o.TransactionType,
                        Date = o.Date,
                        CurrentValue = o.CurrentValue,
                        NewValue = o.NewValue,
                        RiskId = o.RiskId,
                        UserId = o.UserId,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetRiskTransactionForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetRiskTransactionForViewDto> GetRiskTransactionForView(int id)
        {
            var riskTransaction = await _riskTransactionRepository.GetAsync(id);

            var output = new GetRiskTransactionForViewDto { RiskTransaction = ObjectMapper.Map<RiskTransactionDto>(riskTransaction) };

            return output;
        }

        public async Task<GetRiskTransactionForEditOutput> GetRiskTransactionForEdit(EntityDto input)
        {
            var riskTransaction = await _riskTransactionRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRiskTransactionForEditOutput { RiskTransaction = ObjectMapper.Map<CreateOrEditRiskTransactionDto>(riskTransaction) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditRiskTransactionDto input)
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

       
        protected virtual async Task Create(CreateOrEditRiskTransactionDto input)
        {
            var riskTransaction = ObjectMapper.Map<RiskTransaction>(input);

            await _riskTransactionRepository.InsertAsync(riskTransaction);

        }

        
        protected virtual async Task Update(CreateOrEditRiskTransactionDto input)
        {
            var riskTransaction = await _riskTransactionRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, riskTransaction);

        }

        
        public async Task Delete(EntityDto input)
        {
            await _riskTransactionRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetRiskTransactionsToExcel(GetAllRiskTransactionsForExcelInput input)
        {

            var filteredRiskTransactions = _riskTransactionRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TransactionType.Contains(input.Filter) || e.Date.Contains(input.Filter) || e.CurrentValue.Contains(input.Filter) || e.NewValue.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionTypeFilter), e => e.TransactionType == input.TransactionTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DateFilter), e => e.Date == input.DateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrentValueFilter), e => e.CurrentValue == input.CurrentValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NewValueFilter), e => e.NewValue == input.NewValueFilter)
                        .WhereIf(input.MinRiskIdFilter != null, e => e.RiskId >= input.MinRiskIdFilter)
                        .WhereIf(input.MaxRiskIdFilter != null, e => e.RiskId <= input.MaxRiskIdFilter)
                        .WhereIf(input.MinUserIdFilter != null, e => e.UserId >= input.MinUserIdFilter)
                        .WhereIf(input.MaxUserIdFilter != null, e => e.UserId <= input.MaxUserIdFilter);

            var query = (from o in filteredRiskTransactions
                         select new GetRiskTransactionForViewDto()
                         {
                             RiskTransaction = new RiskTransactionDto
                             {
                                 TransactionType = o.TransactionType,
                                 Date = o.Date,
                                 CurrentValue = o.CurrentValue,
                                 NewValue = o.NewValue,
                                 RiskId = o.RiskId,
                                 UserId = o.UserId,
                                 Id = o.Id
                             }
                         });

            var riskTransactionListDtos = await query.ToListAsync();

            return _riskTransactionsExcelExporter.ExportToFile(riskTransactionListDtos);
        }

    }
}