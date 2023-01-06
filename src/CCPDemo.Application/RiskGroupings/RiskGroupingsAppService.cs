using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.RiskGroupings.Exporting;
using CCPDemo.RiskGroupings.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;

namespace CCPDemo.RiskGroupings
{
    [AbpAuthorize(AppPermissions.Pages_Administration_RiskGroupings)]
    public class RiskGroupingsAppService : CCPDemoAppServiceBase, IRiskGroupingsAppService
    {
        private readonly IRepository<RiskGrouping> _riskGroupingRepository;
        private readonly IRiskGroupingsExcelExporter _riskGroupingsExcelExporter;

        public RiskGroupingsAppService(IRepository<RiskGrouping> riskGroupingRepository, IRiskGroupingsExcelExporter riskGroupingsExcelExporter)
        {
            _riskGroupingRepository = riskGroupingRepository;
            _riskGroupingsExcelExporter = riskGroupingsExcelExporter;

        }

        public async Task<PagedResultDto<GetRiskGroupingForViewDto>> GetAll(GetAllRiskGroupingsInput input)
        {

            var filteredRiskGroupings = _riskGroupingRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(input.MinValueFilter != null, e => e.Value >= input.MinValueFilter)
                        .WhereIf(input.MaxValueFilter != null, e => e.Value <= input.MaxValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinDefaultFilter != null, e => e.Default >= input.MinDefaultFilter)
                        .WhereIf(input.MaxDefaultFilter != null, e => e.Default <= input.MaxDefaultFilter)
                        .WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
                        .WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter);

            var pagedAndFilteredRiskGroupings = filteredRiskGroupings
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var riskGroupings = from o in pagedAndFilteredRiskGroupings
                                select new
                                {

                                    o.Value,
                                    o.Name,
                                    o.Default,
                                    o.Order,
                                    Id = o.Id
                                };

            var totalCount = await filteredRiskGroupings.CountAsync();

            var dbList = await riskGroupings.ToListAsync();
            var results = new List<GetRiskGroupingForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetRiskGroupingForViewDto()
                {
                    RiskGrouping = new RiskGroupingDto
                    {

                        Value = o.Value,
                        Name = o.Name,
                        Default = o.Default,
                        Order = o.Order,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetRiskGroupingForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetRiskGroupingForViewDto> GetRiskGroupingForView(int id)
        {
            var riskGrouping = await _riskGroupingRepository.GetAsync(id);

            var output = new GetRiskGroupingForViewDto { RiskGrouping = ObjectMapper.Map<RiskGroupingDto>(riskGrouping) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskGroupings_Edit)]
        public async Task<GetRiskGroupingForEditOutput> GetRiskGroupingForEdit(EntityDto input)
        {
            var riskGrouping = await _riskGroupingRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRiskGroupingForEditOutput { RiskGrouping = ObjectMapper.Map<CreateOrEditRiskGroupingDto>(riskGrouping) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditRiskGroupingDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskGroupings_Create)]
        protected virtual async Task Create(CreateOrEditRiskGroupingDto input)
        {
            var riskGrouping = ObjectMapper.Map<RiskGrouping>(input);

            if (AbpSession.TenantId != null)
            {
                riskGrouping.TenantId = (int?)AbpSession.TenantId;
            }

            await _riskGroupingRepository.InsertAsync(riskGrouping);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskGroupings_Edit)]
        protected virtual async Task Update(CreateOrEditRiskGroupingDto input)
        {
            var riskGrouping = await _riskGroupingRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, riskGrouping);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskGroupings_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _riskGroupingRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetRiskGroupingsToExcel(GetAllRiskGroupingsForExcelInput input)
        {

            var filteredRiskGroupings = _riskGroupingRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(input.MinValueFilter != null, e => e.Value >= input.MinValueFilter)
                        .WhereIf(input.MaxValueFilter != null, e => e.Value <= input.MaxValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinDefaultFilter != null, e => e.Default >= input.MinDefaultFilter)
                        .WhereIf(input.MaxDefaultFilter != null, e => e.Default <= input.MaxDefaultFilter)
                        .WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
                        .WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter);

            var query = (from o in filteredRiskGroupings
                         select new GetRiskGroupingForViewDto()
                         {
                             RiskGrouping = new RiskGroupingDto
                             {
                                 Value = o.Value,
                                 Name = o.Name,
                                 Default = o.Default,
                                 Order = o.Order,
                                 Id = o.Id
                             }
                         });

            var riskGroupingListDtos = await query.ToListAsync();

            return _riskGroupingsExcelExporter.ExportToFile(riskGroupingListDtos);
        }

    }
}