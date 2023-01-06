using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.RiskFunctions.Exporting;
using CCPDemo.RiskFunctions.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;

namespace CCPDemo.RiskFunctions
{
    [AbpAuthorize(AppPermissions.Pages_Administration_RiskFunctions)]
    public class RiskFunctionsAppService : CCPDemoAppServiceBase, IRiskFunctionsAppService
    {
        private readonly IRepository<RiskFunction> _riskFunctionRepository;
        private readonly IRiskFunctionsExcelExporter _riskFunctionsExcelExporter;

        public RiskFunctionsAppService(IRepository<RiskFunction> riskFunctionRepository, IRiskFunctionsExcelExporter riskFunctionsExcelExporter)
        {
            _riskFunctionRepository = riskFunctionRepository;
            _riskFunctionsExcelExporter = riskFunctionsExcelExporter;

        }

        public async Task<PagedResultDto<GetRiskFunctionForViewDto>> GetAll(GetAllRiskFunctionsInput input)
        {

            var filteredRiskFunctions = _riskFunctionRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(input.MinValueFilter != null, e => e.Value >= input.MinValueFilter)
                        .WhereIf(input.MaxValueFilter != null, e => e.Value <= input.MaxValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredRiskFunctions = filteredRiskFunctions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var riskFunctions = from o in pagedAndFilteredRiskFunctions
                                select new
                                {

                                    o.Value,
                                    o.Name,
                                    Id = o.Id
                                };

            var totalCount = await filteredRiskFunctions.CountAsync();

            var dbList = await riskFunctions.ToListAsync();
            var results = new List<GetRiskFunctionForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetRiskFunctionForViewDto()
                {
                    RiskFunction = new RiskFunctionDto
                    {

                        Value = o.Value,
                        Name = o.Name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetRiskFunctionForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetRiskFunctionForViewDto> GetRiskFunctionForView(int id)
        {
            var riskFunction = await _riskFunctionRepository.GetAsync(id);

            var output = new GetRiskFunctionForViewDto { RiskFunction = ObjectMapper.Map<RiskFunctionDto>(riskFunction) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskFunctions_Edit)]
        public async Task<GetRiskFunctionForEditOutput> GetRiskFunctionForEdit(EntityDto input)
        {
            var riskFunction = await _riskFunctionRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRiskFunctionForEditOutput { RiskFunction = ObjectMapper.Map<CreateOrEditRiskFunctionDto>(riskFunction) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditRiskFunctionDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskFunctions_Create)]
        protected virtual async Task Create(CreateOrEditRiskFunctionDto input)
        {
            var riskFunction = ObjectMapper.Map<RiskFunction>(input);

            if (AbpSession.TenantId != null)
            {
                riskFunction.TenantId = (int?)AbpSession.TenantId;
            }

            await _riskFunctionRepository.InsertAsync(riskFunction);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskFunctions_Edit)]
        protected virtual async Task Update(CreateOrEditRiskFunctionDto input)
        {
            var riskFunction = await _riskFunctionRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, riskFunction);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskFunctions_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _riskFunctionRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetRiskFunctionsToExcel(GetAllRiskFunctionsForExcelInput input)
        {

            var filteredRiskFunctions = _riskFunctionRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(input.MinValueFilter != null, e => e.Value >= input.MinValueFilter)
                        .WhereIf(input.MaxValueFilter != null, e => e.Value <= input.MaxValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var query = (from o in filteredRiskFunctions
                         select new GetRiskFunctionForViewDto()
                         {
                             RiskFunction = new RiskFunctionDto
                             {
                                 Value = o.Value,
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var riskFunctionListDtos = await query.ToListAsync();

            return _riskFunctionsExcelExporter.ExportToFile(riskFunctionListDtos);
        }

    }
}