using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.RiskCloseReason.Exporting;
using CCPDemo.RiskCloseReason.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;

namespace CCPDemo.RiskCloseReason
{
    [AbpAuthorize(AppPermissions.Pages_Administration_CloseReasons)]
    public class CloseReasonsAppService : CCPDemoAppServiceBase, ICloseReasonsAppService
    {
        private readonly IRepository<CloseReason> _closeReasonRepository;
        private readonly ICloseReasonsExcelExporter _closeReasonsExcelExporter;

        public CloseReasonsAppService(IRepository<CloseReason> closeReasonRepository, ICloseReasonsExcelExporter closeReasonsExcelExporter)
        {
            _closeReasonRepository = closeReasonRepository;
            _closeReasonsExcelExporter = closeReasonsExcelExporter;

        }

        public async Task<PagedResultDto<GetCloseReasonForViewDto>> GetAll(GetAllCloseReasonsInput input)
        {

            var filteredCloseReasons = _closeReasonRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.name.Contains(input.Filter))
                        .WhereIf(input.MinvalueFilter != null, e => e.value >= input.MinvalueFilter)
                        .WhereIf(input.MaxvalueFilter != null, e => e.value <= input.MaxvalueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.nameFilter), e => e.name.Contains(input.nameFilter));

            var pagedAndFilteredCloseReasons = filteredCloseReasons
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var closeReasons = from o in pagedAndFilteredCloseReasons
                               select new
                               {

                                   o.value,
                                   o.name,
                                   Id = o.Id
                               };

            var totalCount = await filteredCloseReasons.CountAsync();

            var dbList = await closeReasons.ToListAsync();
            var results = new List<GetCloseReasonForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCloseReasonForViewDto()
                {
                    CloseReason = new CloseReasonDto
                    {

                        value = o.value,
                        name = o.name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCloseReasonForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCloseReasonForViewDto> GetCloseReasonForView(int id)
        {
            var closeReason = await _closeReasonRepository.GetAsync(id);

            var output = new GetCloseReasonForViewDto { CloseReason = ObjectMapper.Map<CloseReasonDto>(closeReason) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_CloseReasons_Edit)]
        public async Task<GetCloseReasonForEditOutput> GetCloseReasonForEdit(EntityDto input)
        {
            var closeReason = await _closeReasonRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCloseReasonForEditOutput { CloseReason = ObjectMapper.Map<CreateOrEditCloseReasonDto>(closeReason) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCloseReasonDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_CloseReasons_Create)]
        protected virtual async Task Create(CreateOrEditCloseReasonDto input)
        {
            var closeReason = ObjectMapper.Map<CloseReason>(input);

            if (AbpSession.TenantId != null)
            {
                closeReason.TenantId = (int?)AbpSession.TenantId;
            }

            await _closeReasonRepository.InsertAsync(closeReason);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_CloseReasons_Edit)]
        protected virtual async Task Update(CreateOrEditCloseReasonDto input)
        {
            var closeReason = await _closeReasonRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, closeReason);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_CloseReasons_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _closeReasonRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCloseReasonsToExcel(GetAllCloseReasonsForExcelInput input)
        {

            var filteredCloseReasons = _closeReasonRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.name.Contains(input.Filter))
                        .WhereIf(input.MinvalueFilter != null, e => e.value >= input.MinvalueFilter)
                        .WhereIf(input.MaxvalueFilter != null, e => e.value <= input.MaxvalueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.nameFilter), e => e.name.Contains(input.nameFilter));

            var query = (from o in filteredCloseReasons
                         select new GetCloseReasonForViewDto()
                         {
                             CloseReason = new CloseReasonDto
                             {
                                 value = o.value,
                                 name = o.name,
                                 Id = o.Id
                             }
                         });

            var closeReasonListDtos = await query.ToListAsync();

            return _closeReasonsExcelExporter.ExportToFile(closeReasonListDtos);
        }

    }
}