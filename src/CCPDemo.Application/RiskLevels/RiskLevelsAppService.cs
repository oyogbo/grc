using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.RiskLevels.Exporting;
using CCPDemo.RiskLevels.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;

namespace CCPDemo.RiskLevels
{
    [AbpAuthorize(AppPermissions.Pages_Administration_RiskLevels)]
    public class RiskLevelsAppService : CCPDemoAppServiceBase, IRiskLevelsAppService
    {
        private readonly IRepository<RiskLevel> _riskLevelRepository;
        private readonly IRiskLevelsExcelExporter _riskLevelsExcelExporter;

        public RiskLevelsAppService(IRepository<RiskLevel> riskLevelRepository, IRiskLevelsExcelExporter riskLevelsExcelExporter)
        {
            _riskLevelRepository = riskLevelRepository;
            _riskLevelsExcelExporter = riskLevelsExcelExporter;

        }

        public async Task<PagedResultDto<GetRiskLevelForViewDto>> GetAll(GetAllRiskLevelsInput input)
        {

            var filteredRiskLevels = _riskLevelRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Color.Contains(input.Filter) || e.DisplayName.Contains(input.Filter))
                        .WhereIf(input.MinValueFilter != null, e => e.Value >= input.MinValueFilter)
                        .WhereIf(input.MaxValueFilter != null, e => e.Value <= input.MaxValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ColorFilter), e => e.Color.Contains(input.ColorFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DisplayNameFilter), e => e.DisplayName.Contains(input.DisplayNameFilter));

            var pagedAndFilteredRiskLevels = filteredRiskLevels
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var riskLevels = from o in pagedAndFilteredRiskLevels
                             select new
                             {

                                 o.Value,
                                 o.Name,
                                 o.Color,
                                 o.DisplayName,
                                 Id = o.Id
                             };

            var totalCount = await filteredRiskLevels.CountAsync();

            var dbList = await riskLevels.ToListAsync();
            var results = new List<GetRiskLevelForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetRiskLevelForViewDto()
                {
                    RiskLevel = new RiskLevelDto
                    {

                        Value = o.Value,
                        Name = o.Name,
                        Color = o.Color,
                        DisplayName = o.DisplayName,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetRiskLevelForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetRiskLevelForViewDto> GetRiskLevelForView(int id)
        {
            var riskLevel = await _riskLevelRepository.GetAsync(id);

            var output = new GetRiskLevelForViewDto { RiskLevel = ObjectMapper.Map<RiskLevelDto>(riskLevel) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskLevels_Edit)]
        public async Task<GetRiskLevelForEditOutput> GetRiskLevelForEdit(EntityDto input)
        {
            var riskLevel = await _riskLevelRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRiskLevelForEditOutput { RiskLevel = ObjectMapper.Map<CreateOrEditRiskLevelDto>(riskLevel) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditRiskLevelDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskLevels_Create)]
        protected virtual async Task Create(CreateOrEditRiskLevelDto input)
        {
            var riskLevel = ObjectMapper.Map<RiskLevel>(input);

            if (AbpSession.TenantId != null)
            {
                riskLevel.TenantId = (int?)AbpSession.TenantId;
            }

            await _riskLevelRepository.InsertAsync(riskLevel);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskLevels_Edit)]
        protected virtual async Task Update(CreateOrEditRiskLevelDto input)
        {
            var riskLevel = await _riskLevelRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, riskLevel);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskLevels_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _riskLevelRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetRiskLevelsToExcel(GetAllRiskLevelsForExcelInput input)
        {

            var filteredRiskLevels = _riskLevelRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Color.Contains(input.Filter) || e.DisplayName.Contains(input.Filter))
                        .WhereIf(input.MinValueFilter != null, e => e.Value >= input.MinValueFilter)
                        .WhereIf(input.MaxValueFilter != null, e => e.Value <= input.MaxValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ColorFilter), e => e.Color.Contains(input.ColorFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DisplayNameFilter), e => e.DisplayName.Contains(input.DisplayNameFilter));

            var query = (from o in filteredRiskLevels
                         select new GetRiskLevelForViewDto()
                         {
                             RiskLevel = new RiskLevelDto
                             {
                                 Value = o.Value,
                                 Name = o.Name,
                                 Color = o.Color,
                                 DisplayName = o.DisplayName,
                                 Id = o.Id
                             }
                         });

            var riskLevelListDtos = await query.ToListAsync();

            return _riskLevelsExcelExporter.ExportToFile(riskLevelListDtos);
        }

    }
}