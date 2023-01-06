using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.RiskModels.Exporting;
using CCPDemo.RiskModels.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;

namespace CCPDemo.RiskModels
{
    [AbpAuthorize(AppPermissions.Pages_Administration_RiskModels)]
    public class RiskModelsAppService : CCPDemoAppServiceBase, IRiskModelsAppService
    {
        private readonly IRepository<RiskModel> _riskModelRepository;
        private readonly IRiskModelsExcelExporter _riskModelsExcelExporter;

        public RiskModelsAppService(IRepository<RiskModel> riskModelRepository, IRiskModelsExcelExporter riskModelsExcelExporter)
        {
            _riskModelRepository = riskModelRepository;
            _riskModelsExcelExporter = riskModelsExcelExporter;

        }

        public async Task<PagedResultDto<GetRiskModelForViewDto>> GetAll(GetAllRiskModelsInput input)
        {

            var filteredRiskModels = _riskModelRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(input.MinValueFilter != null, e => e.Value >= input.MinValueFilter)
                        .WhereIf(input.MaxValueFilter != null, e => e.Value <= input.MaxValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredRiskModels = filteredRiskModels
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var riskModels = from o in pagedAndFilteredRiskModels
                             select new
                             {

                                 o.Value,
                                 o.Name,
                                 Id = o.Id
                             };

            var totalCount = await filteredRiskModels.CountAsync();

            var dbList = await riskModels.ToListAsync();
            var results = new List<GetRiskModelForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetRiskModelForViewDto()
                {
                    RiskModel = new RiskModelDto
                    {

                        Value = o.Value,
                        Name = o.Name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetRiskModelForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetRiskModelForViewDto> GetRiskModelForView(int id)
        {
            var riskModel = await _riskModelRepository.GetAsync(id);

            var output = new GetRiskModelForViewDto { RiskModel = ObjectMapper.Map<RiskModelDto>(riskModel) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskModels_Edit)]
        public async Task<GetRiskModelForEditOutput> GetRiskModelForEdit(EntityDto input)
        {
            var riskModel = await _riskModelRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRiskModelForEditOutput { RiskModel = ObjectMapper.Map<CreateOrEditRiskModelDto>(riskModel) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditRiskModelDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskModels_Create)]
        protected virtual async Task Create(CreateOrEditRiskModelDto input)
        {
            var riskModel = ObjectMapper.Map<RiskModel>(input);

            if (AbpSession.TenantId != null)
            {
                riskModel.TenantId = (int?)AbpSession.TenantId;
            }

            await _riskModelRepository.InsertAsync(riskModel);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskModels_Edit)]
        protected virtual async Task Update(CreateOrEditRiskModelDto input)
        {
            var riskModel = await _riskModelRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, riskModel);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskModels_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _riskModelRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetRiskModelsToExcel(GetAllRiskModelsForExcelInput input)
        {

            var filteredRiskModels = _riskModelRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(input.MinValueFilter != null, e => e.Value >= input.MinValueFilter)
                        .WhereIf(input.MaxValueFilter != null, e => e.Value <= input.MaxValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var query = (from o in filteredRiskModels
                         select new GetRiskModelForViewDto()
                         {
                             RiskModel = new RiskModelDto
                             {
                                 Value = o.Value,
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var riskModelListDtos = await query.ToListAsync();

            return _riskModelsExcelExporter.ExportToFile(riskModelListDtos);
        }

    }
}