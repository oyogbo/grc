using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.RiskTypes.Exporting;
using CCPDemo.RiskTypes.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;

namespace CCPDemo.RiskTypes
{
    [AbpAuthorize(AppPermissions.Pages_Administration_RiskTypes)]
    public class RiskTypesAppService : CCPDemoAppServiceBase, IRiskTypesAppService
    {
        private readonly IRepository<RiskType> _riskTypeRepository;
        private readonly IRiskTypesExcelExporter _riskTypesExcelExporter;

        public RiskTypesAppService(IRepository<RiskType> riskTypeRepository, IRiskTypesExcelExporter riskTypesExcelExporter)
        {
            _riskTypeRepository = riskTypeRepository;
            _riskTypesExcelExporter = riskTypesExcelExporter;

        }

        public async Task<PagedResultDto<GetRiskTypeForViewDto>> GetAll(GetAllRiskTypesInput input)
        {

            var filteredRiskTypes = _riskTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var pagedAndFilteredRiskTypes = filteredRiskTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var riskTypes = from o in pagedAndFilteredRiskTypes
                            select new
                            {

                                o.Name,
                                Id = o.Id
                            };

            var totalCount = await filteredRiskTypes.CountAsync();

            var dbList = await riskTypes.ToListAsync();
            var results = new List<GetRiskTypeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetRiskTypeForViewDto()
                {
                    RiskType = new RiskTypeDto
                    {

                        Name = o.Name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetRiskTypeForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetRiskTypeForViewDto> GetRiskTypeForView(int id)
        {
            var riskType = await _riskTypeRepository.GetAsync(id);

            var output = new GetRiskTypeForViewDto { RiskType = ObjectMapper.Map<RiskTypeDto>(riskType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskTypes_Edit)]
        public async Task<GetRiskTypeForEditOutput> GetRiskTypeForEdit(EntityDto input)
        {
            var riskType = await _riskTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRiskTypeForEditOutput { RiskType = ObjectMapper.Map<CreateOrEditRiskTypeDto>(riskType) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditRiskTypeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskTypes_Create)]
        protected virtual async Task Create(CreateOrEditRiskTypeDto input)
        {
            var riskType = ObjectMapper.Map<RiskType>(input);

            await _riskTypeRepository.InsertAsync(riskType);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskTypes_Edit)]
        protected virtual async Task Update(CreateOrEditRiskTypeDto input)
        {
            var riskType = await _riskTypeRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, riskType);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskTypes_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _riskTypeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetRiskTypesToExcel(GetAllRiskTypesForExcelInput input)
        {

            var filteredRiskTypes = _riskTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var query = (from o in filteredRiskTypes
                         select new GetRiskTypeForViewDto()
                         {
                             RiskType = new RiskTypeDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var riskTypeListDtos = await query.ToListAsync();

            return _riskTypesExcelExporter.ExportToFile(riskTypeListDtos);
        }

    }
}