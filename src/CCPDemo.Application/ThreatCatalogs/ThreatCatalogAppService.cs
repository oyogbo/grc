using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.ThreatCatalogs.Exporting;
using CCPDemo.ThreatCatalogs.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;

namespace CCPDemo.ThreatCatalogs
{
    [AbpAuthorize(AppPermissions.Pages_Administration_ThreatCatalog)]
    public class ThreatCatalogAppService : CCPDemoAppServiceBase, IThreatCatalogAppService
    {
        private readonly IRepository<ThreatCatalog> _threatCatalogRepository;
        private readonly IThreatCatalogExcelExporter _threatCatalogExcelExporter;

        public ThreatCatalogAppService(IRepository<ThreatCatalog> threatCatalogRepository, IThreatCatalogExcelExporter threatCatalogExcelExporter)
        {
            _threatCatalogRepository = threatCatalogRepository;
            _threatCatalogExcelExporter = threatCatalogExcelExporter;

        }

        public async Task<PagedResultDto<GetThreatCatalogForViewDto>> GetAll(GetAllThreatCatalogInput input)
        {

            var filteredThreatCatalog = _threatCatalogRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Number.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NumberFilter), e => e.Number.Contains(input.NumberFilter))
                        .WhereIf(input.MinGroupingFilter != null, e => e.Grouping >= input.MinGroupingFilter)
                        .WhereIf(input.MaxGroupingFilter != null, e => e.Grouping <= input.MaxGroupingFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
                        .WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter);

            var pagedAndFilteredThreatCatalog = filteredThreatCatalog
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var threatCatalog = from o in pagedAndFilteredThreatCatalog
                                select new
                                {

                                    o.Number,
                                    o.Grouping,
                                    o.Name,
                                    o.Description,
                                    o.Order,
                                    Id = o.Id
                                };

            var totalCount = await filteredThreatCatalog.CountAsync();

            var dbList = await threatCatalog.ToListAsync();
            var results = new List<GetThreatCatalogForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetThreatCatalogForViewDto()
                {
                    ThreatCatalog = new ThreatCatalogDto
                    {

                        Number = o.Number,
                        Grouping = o.Grouping,
                        Name = o.Name,
                        Description = o.Description,
                        Order = o.Order,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetThreatCatalogForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetThreatCatalogForViewDto> GetThreatCatalogForView(int id)
        {
            var threatCatalog = await _threatCatalogRepository.GetAsync(id);

            var output = new GetThreatCatalogForViewDto { ThreatCatalog = ObjectMapper.Map<ThreatCatalogDto>(threatCatalog) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ThreatCatalog_Edit)]
        public async Task<GetThreatCatalogForEditOutput> GetThreatCatalogForEdit(EntityDto input)
        {
            var threatCatalog = await _threatCatalogRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetThreatCatalogForEditOutput { ThreatCatalog = ObjectMapper.Map<CreateOrEditThreatCatalogDto>(threatCatalog) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditThreatCatalogDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_ThreatCatalog_Create)]
        protected virtual async Task Create(CreateOrEditThreatCatalogDto input)
        {
            var threatCatalog = ObjectMapper.Map<ThreatCatalog>(input);

            if (AbpSession.TenantId != null)
            {
                threatCatalog.TenantId = (int?)AbpSession.TenantId;
            }

            await _threatCatalogRepository.InsertAsync(threatCatalog);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ThreatCatalog_Edit)]
        protected virtual async Task Update(CreateOrEditThreatCatalogDto input)
        {
            var threatCatalog = await _threatCatalogRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, threatCatalog);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ThreatCatalog_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _threatCatalogRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetThreatCatalogToExcel(GetAllThreatCatalogForExcelInput input)
        {

            var filteredThreatCatalog = _threatCatalogRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Number.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NumberFilter), e => e.Number.Contains(input.NumberFilter))
                        .WhereIf(input.MinGroupingFilter != null, e => e.Grouping >= input.MinGroupingFilter)
                        .WhereIf(input.MaxGroupingFilter != null, e => e.Grouping <= input.MaxGroupingFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
                        .WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter);

            var query = (from o in filteredThreatCatalog
                         select new GetThreatCatalogForViewDto()
                         {
                             ThreatCatalog = new ThreatCatalogDto
                             {
                                 Number = o.Number,
                                 Grouping = o.Grouping,
                                 Name = o.Name,
                                 Description = o.Description,
                                 Order = o.Order,
                                 Id = o.Id
                             }
                         });

            var threatCatalogListDtos = await query.ToListAsync();

            return _threatCatalogExcelExporter.ExportToFile(threatCatalogListDtos);
        }

    }
}