using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.ThreatGroupings.Exporting;
using CCPDemo.ThreatGroupings.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;

namespace CCPDemo.ThreatGroupings
{
    [AbpAuthorize(AppPermissions.Pages_Administration_ThreatGrouping)]
    public class ThreatGroupingAppService : CCPDemoAppServiceBase, IThreatGroupingAppService
    {
        private readonly IRepository<ThreatGrouping> _threatGroupingRepository;
        private readonly IThreatGroupingExcelExporter _threatGroupingExcelExporter;

        public ThreatGroupingAppService(IRepository<ThreatGrouping> threatGroupingRepository, IThreatGroupingExcelExporter threatGroupingExcelExporter)
        {
            _threatGroupingRepository = threatGroupingRepository;
            _threatGroupingExcelExporter = threatGroupingExcelExporter;

        }

        public async Task<PagedResultDto<GetThreatGroupingForViewDto>> GetAll(GetAllThreatGroupingInput input)
        {

            var filteredThreatGrouping = _threatGroupingRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(input.MinValueFilter != null, e => e.Value >= input.MinValueFilter)
                        .WhereIf(input.MaxValueFilter != null, e => e.Value <= input.MaxValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinDefaultFilter != null, e => e.Default >= input.MinDefaultFilter)
                        .WhereIf(input.MaxDefaultFilter != null, e => e.Default <= input.MaxDefaultFilter)
                        .WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
                        .WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter);

            var pagedAndFilteredThreatGrouping = filteredThreatGrouping
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var threatGrouping = from o in pagedAndFilteredThreatGrouping
                                 select new
                                 {

                                     o.Value,
                                     o.Name,
                                     o.Default,
                                     o.Order,
                                     Id = o.Id
                                 };

            var totalCount = await filteredThreatGrouping.CountAsync();

            var dbList = await threatGrouping.ToListAsync();
            var results = new List<GetThreatGroupingForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetThreatGroupingForViewDto()
                {
                    ThreatGrouping = new ThreatGroupingDto
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

            return new PagedResultDto<GetThreatGroupingForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetThreatGroupingForViewDto> GetThreatGroupingForView(int id)
        {
            var threatGrouping = await _threatGroupingRepository.GetAsync(id);

            var output = new GetThreatGroupingForViewDto { ThreatGrouping = ObjectMapper.Map<ThreatGroupingDto>(threatGrouping) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ThreatGrouping_Edit)]
        public async Task<GetThreatGroupingForEditOutput> GetThreatGroupingForEdit(EntityDto input)
        {
            var threatGrouping = await _threatGroupingRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetThreatGroupingForEditOutput { ThreatGrouping = ObjectMapper.Map<CreateOrEditThreatGroupingDto>(threatGrouping) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditThreatGroupingDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_ThreatGrouping_Create)]
        protected virtual async Task Create(CreateOrEditThreatGroupingDto input)
        {
            var threatGrouping = ObjectMapper.Map<ThreatGrouping>(input);

            if (AbpSession.TenantId != null)
            {
                threatGrouping.TenantId = (int?)AbpSession.TenantId;
            }

            await _threatGroupingRepository.InsertAsync(threatGrouping);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ThreatGrouping_Edit)]
        protected virtual async Task Update(CreateOrEditThreatGroupingDto input)
        {
            var threatGrouping = await _threatGroupingRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, threatGrouping);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ThreatGrouping_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _threatGroupingRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetThreatGroupingToExcel(GetAllThreatGroupingForExcelInput input)
        {

            var filteredThreatGrouping = _threatGroupingRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(input.MinValueFilter != null, e => e.Value >= input.MinValueFilter)
                        .WhereIf(input.MaxValueFilter != null, e => e.Value <= input.MaxValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinDefaultFilter != null, e => e.Default >= input.MinDefaultFilter)
                        .WhereIf(input.MaxDefaultFilter != null, e => e.Default <= input.MaxDefaultFilter)
                        .WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
                        .WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter);

            var query = (from o in filteredThreatGrouping
                         select new GetThreatGroupingForViewDto()
                         {
                             ThreatGrouping = new ThreatGroupingDto
                             {
                                 Value = o.Value,
                                 Name = o.Name,
                                 Default = o.Default,
                                 Order = o.Order,
                                 Id = o.Id
                             }
                         });

            var threatGroupingListDtos = await query.ToListAsync();

            return _threatGroupingExcelExporter.ExportToFile(threatGroupingListDtos);
        }

    }
}