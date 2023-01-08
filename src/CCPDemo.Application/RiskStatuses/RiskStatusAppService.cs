using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.RiskStatuses.Exporting;
using CCPDemo.RiskStatuses.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;

namespace CCPDemo.RiskStatuses
{
    [AbpAuthorize(AppPermissions.Pages_Administration_RiskStatus)]
    public class RiskStatusAppService : CCPDemoAppServiceBase, IRiskStatusAppService
    {
        private readonly IRepository<RiskStatus> _riskStatusRepository;
        private readonly IRiskStatusExcelExporter _riskStatusExcelExporter;

        public RiskStatusAppService(IRepository<RiskStatus> riskStatusRepository, IRiskStatusExcelExporter riskStatusExcelExporter)
        {
            _riskStatusRepository = riskStatusRepository;
            _riskStatusExcelExporter = riskStatusExcelExporter;

        }

        public async Task<PagedResultDto<GetRiskStatusForViewDto>> GetAll(GetAllRiskStatusInput input)
        {

            var filteredRiskStatus = _riskStatusRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var pagedAndFilteredRiskStatus = filteredRiskStatus
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var riskStatus = from o in pagedAndFilteredRiskStatus
                             select new
                             {

                                 o.Name,
                                 Id = o.Id
                             };

            var totalCount = await filteredRiskStatus.CountAsync();

            var dbList = await riskStatus.ToListAsync();
            var results = new List<GetRiskStatusForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetRiskStatusForViewDto()
                {
                    RiskStatus = new RiskStatusDto
                    {

                        Name = o.Name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetRiskStatusForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetRiskStatusForViewDto> GetRiskStatusForView(int id)
        {
            var riskStatus = await _riskStatusRepository.GetAsync(id);

            var output = new GetRiskStatusForViewDto { RiskStatus = ObjectMapper.Map<RiskStatusDto>(riskStatus) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskStatus_Edit)]
        public async Task<GetRiskStatusForEditOutput> GetRiskStatusForEdit(EntityDto input)
        {
            var riskStatus = await _riskStatusRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRiskStatusForEditOutput { RiskStatus = ObjectMapper.Map<CreateOrEditRiskStatusDto>(riskStatus) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditRiskStatusDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskStatus_Create)]
        protected virtual async Task Create(CreateOrEditRiskStatusDto input)
        {
            var riskStatus = ObjectMapper.Map<RiskStatus>(input);

            await _riskStatusRepository.InsertAsync(riskStatus);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskStatus_Edit)]
        protected virtual async Task Update(CreateOrEditRiskStatusDto input)
        {
            var riskStatus = await _riskStatusRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, riskStatus);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskStatus_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _riskStatusRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetRiskStatusToExcel(GetAllRiskStatusForExcelInput input)
        {

            var filteredRiskStatus = _riskStatusRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var query = (from o in filteredRiskStatus
                         select new GetRiskStatusForViewDto()
                         {
                             RiskStatus = new RiskStatusDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var riskStatusListDtos = await query.ToListAsync();

            return _riskStatusExcelExporter.ExportToFile(riskStatusListDtos);
        }

    }
}