using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.RiskStatus.Exporting;
using CCPDemo.RiskStatus.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;

namespace CCPDemo.RiskStatus
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Status)]
    public class StatusAppService : CCPDemoAppServiceBase, IStatusAppService
    {
        private readonly IRepository<Status> _statusRepository;
        private readonly IStatusExcelExporter _statusExcelExporter;

        public StatusAppService(IRepository<Status> statusRepository, IStatusExcelExporter statusExcelExporter)
        {
            _statusRepository = statusRepository;
            _statusExcelExporter = statusExcelExporter;

        }

        public async Task<PagedResultDto<GetStatusForViewDto>> GetAll(GetAllStatusInput input)
        {

            var filteredStatus = _statusRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var pagedAndFilteredStatus = filteredStatus
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var status = from o in pagedAndFilteredStatus
                         select new
                         {

                             o.Name,
                             Id = o.Id
                         };

            var totalCount = await filteredStatus.CountAsync();

            var dbList = await status.ToListAsync();
            var results = new List<GetStatusForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStatusForViewDto()
                {
                    Status = new StatusDto
                    {

                        Name = o.Name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStatusForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStatusForViewDto> GetStatusForView(int id)
        {
            var status = await _statusRepository.GetAsync(id);

            var output = new GetStatusForViewDto { Status = ObjectMapper.Map<StatusDto>(status) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Status_Edit)]
        public async Task<GetStatusForEditOutput> GetStatusForEdit(EntityDto input)
        {
            var status = await _statusRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStatusForEditOutput { Status = ObjectMapper.Map<CreateOrEditStatusDto>(status) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStatusDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_Status_Create)]
        protected virtual async Task Create(CreateOrEditStatusDto input)
        {
            var status = ObjectMapper.Map<Status>(input);

            await _statusRepository.InsertAsync(status);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Status_Edit)]
        protected virtual async Task Update(CreateOrEditStatusDto input)
        {
            var status = await _statusRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, status);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Status_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _statusRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStatusToExcel(GetAllStatusForExcelInput input)
        {

            var filteredStatus = _statusRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var query = (from o in filteredStatus
                         select new GetStatusForViewDto()
                         {
                             Status = new StatusDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var statusListDtos = await query.ToListAsync();

            return _statusExcelExporter.ExportToFile(statusListDtos);
        }

    }
}