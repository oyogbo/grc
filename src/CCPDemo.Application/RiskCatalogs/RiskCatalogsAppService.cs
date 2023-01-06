using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.RiskCatalogs.Exporting;
using CCPDemo.RiskCatalogs.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;

namespace CCPDemo.RiskCatalogs
{
    [AbpAuthorize(AppPermissions.Pages_Administration_RiskCatalogs)]
    public class RiskCatalogsAppService : CCPDemoAppServiceBase, IRiskCatalogsAppService
    {
        private readonly IRepository<RiskCatalog> _riskCatalogRepository;
        private readonly IRiskCatalogsExcelExporter _riskCatalogsExcelExporter;

        public RiskCatalogsAppService(IRepository<RiskCatalog> riskCatalogRepository, IRiskCatalogsExcelExporter riskCatalogsExcelExporter)
        {
            _riskCatalogRepository = riskCatalogRepository;
            _riskCatalogsExcelExporter = riskCatalogsExcelExporter;

        }

        public async Task<PagedResultDto<GetRiskCatalogForViewDto>> GetAll(GetAllRiskCatalogsInput input)
        {

            var filteredRiskCatalogs = _riskCatalogRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Number.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NumberFilter), e => e.Number.Contains(input.NumberFilter))
                        .WhereIf(input.MinGroupingFilter != null, e => e.Grouping >= input.MinGroupingFilter)
                        .WhereIf(input.MaxGroupingFilter != null, e => e.Grouping <= input.MaxGroupingFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.MinFunctionFilter != null, e => e.Function >= input.MinFunctionFilter)
                        .WhereIf(input.MaxFunctionFilter != null, e => e.Function <= input.MaxFunctionFilter)
                        .WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
                        .WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter);

            var pagedAndFilteredRiskCatalogs = filteredRiskCatalogs
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var riskCatalogs = from o in pagedAndFilteredRiskCatalogs
                               select new
                               {

                                   o.Number,
                                   o.Grouping,
                                   o.Name,
                                   o.Description,
                                   o.Function,
                                   o.Order,
                                   Id = o.Id
                               };

            var totalCount = await filteredRiskCatalogs.CountAsync();

            var dbList = await riskCatalogs.ToListAsync();
            var results = new List<GetRiskCatalogForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetRiskCatalogForViewDto()
                {
                    RiskCatalog = new RiskCatalogDto
                    {

                        Number = o.Number,
                        Grouping = o.Grouping,
                        Name = o.Name,
                        Description = o.Description,
                        Function = o.Function,
                        Order = o.Order,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetRiskCatalogForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetRiskCatalogForViewDto> GetRiskCatalogForView(int id)
        {
            var riskCatalog = await _riskCatalogRepository.GetAsync(id);

            var output = new GetRiskCatalogForViewDto { RiskCatalog = ObjectMapper.Map<RiskCatalogDto>(riskCatalog) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskCatalogs_Edit)]
        public async Task<GetRiskCatalogForEditOutput> GetRiskCatalogForEdit(EntityDto input)
        {
            var riskCatalog = await _riskCatalogRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRiskCatalogForEditOutput { RiskCatalog = ObjectMapper.Map<CreateOrEditRiskCatalogDto>(riskCatalog) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditRiskCatalogDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskCatalogs_Create)]
        protected virtual async Task Create(CreateOrEditRiskCatalogDto input)
        {
            var riskCatalog = ObjectMapper.Map<RiskCatalog>(input);

            if (AbpSession.TenantId != null)
            {
                riskCatalog.TenantId = (int?)AbpSession.TenantId;
            }

            await _riskCatalogRepository.InsertAsync(riskCatalog);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskCatalogs_Edit)]
        protected virtual async Task Update(CreateOrEditRiskCatalogDto input)
        {
            var riskCatalog = await _riskCatalogRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, riskCatalog);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskCatalogs_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _riskCatalogRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetRiskCatalogsToExcel(GetAllRiskCatalogsForExcelInput input)
        {

            var filteredRiskCatalogs = _riskCatalogRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Number.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NumberFilter), e => e.Number.Contains(input.NumberFilter))
                        .WhereIf(input.MinGroupingFilter != null, e => e.Grouping >= input.MinGroupingFilter)
                        .WhereIf(input.MaxGroupingFilter != null, e => e.Grouping <= input.MaxGroupingFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.MinFunctionFilter != null, e => e.Function >= input.MinFunctionFilter)
                        .WhereIf(input.MaxFunctionFilter != null, e => e.Function <= input.MaxFunctionFilter)
                        .WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
                        .WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter);

            var query = (from o in filteredRiskCatalogs
                         select new GetRiskCatalogForViewDto()
                         {
                             RiskCatalog = new RiskCatalogDto
                             {
                                 Number = o.Number,
                                 Grouping = o.Grouping,
                                 Name = o.Name,
                                 Description = o.Description,
                                 Function = o.Function,
                                 Order = o.Order,
                                 Id = o.Id
                             }
                         });

            var riskCatalogListDtos = await query.ToListAsync();

            return _riskCatalogsExcelExporter.ExportToFile(riskCatalogListDtos);
        }

    }
}