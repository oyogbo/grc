using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.Closures.Exporting;
using CCPDemo.Closures.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;

namespace CCPDemo.Closures
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Closures)]
    public class ClosuresAppService : CCPDemoAppServiceBase, IClosuresAppService
    {
        private readonly IRepository<Closure> _closureRepository;
        private readonly IClosuresExcelExporter _closuresExcelExporter;

        public ClosuresAppService(IRepository<Closure> closureRepository, IClosuresExcelExporter closuresExcelExporter)
        {
            _closureRepository = closureRepository;
            _closuresExcelExporter = closuresExcelExporter;

        }

        public async Task<PagedResultDto<GetClosureForViewDto>> GetAll(GetAllClosuresInput input)
        {

            var filteredClosures = _closureRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Note.Contains(input.Filter))
                        .WhereIf(input.MinRiskIdFilter != null, e => e.RiskId >= input.MinRiskIdFilter)
                        .WhereIf(input.MaxRiskIdFilter != null, e => e.RiskId <= input.MaxRiskIdFilter)
                        .WhereIf(input.MinUserIdFilter != null, e => e.UserId >= input.MinUserIdFilter)
                        .WhereIf(input.MaxUserIdFilter != null, e => e.UserId <= input.MaxUserIdFilter)
                        .WhereIf(input.MinClosureDateFilter != null, e => e.ClosureDate >= input.MinClosureDateFilter)
                        .WhereIf(input.MaxClosureDateFilter != null, e => e.ClosureDate <= input.MaxClosureDateFilter)
                        .WhereIf(input.MinCloseReasonIdFilter != null, e => e.CloseReasonId >= input.MinCloseReasonIdFilter)
                        .WhereIf(input.MaxCloseReasonIdFilter != null, e => e.CloseReasonId <= input.MaxCloseReasonIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NoteFilter), e => e.Note.Contains(input.NoteFilter));

            var pagedAndFilteredClosures = filteredClosures
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var closures = from o in pagedAndFilteredClosures
                           select new
                           {

                               o.RiskId,
                               o.UserId,
                               o.ClosureDate,
                               o.CloseReasonId,
                               o.Note,
                               Id = o.Id
                           };

            var totalCount = await filteredClosures.CountAsync();

            var dbList = await closures.ToListAsync();
            var results = new List<GetClosureForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetClosureForViewDto()
                {
                    Closure = new ClosureDto
                    {

                        RiskId = o.RiskId,
                        UserId = o.UserId,
                        ClosureDate = o.ClosureDate,
                        CloseReasonId = o.CloseReasonId,
                        Note = o.Note,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetClosureForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetClosureForViewDto> GetClosureForView(int id)
        {
            var closure = await _closureRepository.GetAsync(id);

            var output = new GetClosureForViewDto { Closure = ObjectMapper.Map<ClosureDto>(closure) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Closures_Edit)]
        public async Task<GetClosureForEditOutput> GetClosureForEdit(EntityDto input)
        {
            var closure = await _closureRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetClosureForEditOutput { Closure = ObjectMapper.Map<CreateOrEditClosureDto>(closure) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditClosureDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_Closures_Create)]
        protected virtual async Task Create(CreateOrEditClosureDto input)
        {
            var closure = ObjectMapper.Map<Closure>(input);

            if (AbpSession.TenantId != null)
            {
                closure.TenantId = (int?)AbpSession.TenantId;
            }

            await _closureRepository.InsertAsync(closure);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Closures_Edit)]
        protected virtual async Task Update(CreateOrEditClosureDto input)
        {
            var closure = await _closureRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, closure);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Closures_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _closureRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetClosuresToExcel(GetAllClosuresForExcelInput input)
        {

            var filteredClosures = _closureRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Note.Contains(input.Filter))
                        .WhereIf(input.MinRiskIdFilter != null, e => e.RiskId >= input.MinRiskIdFilter)
                        .WhereIf(input.MaxRiskIdFilter != null, e => e.RiskId <= input.MaxRiskIdFilter)
                        .WhereIf(input.MinUserIdFilter != null, e => e.UserId >= input.MinUserIdFilter)
                        .WhereIf(input.MaxUserIdFilter != null, e => e.UserId <= input.MaxUserIdFilter)
                        .WhereIf(input.MinClosureDateFilter != null, e => e.ClosureDate >= input.MinClosureDateFilter)
                        .WhereIf(input.MaxClosureDateFilter != null, e => e.ClosureDate <= input.MaxClosureDateFilter)
                        .WhereIf(input.MinCloseReasonIdFilter != null, e => e.CloseReasonId >= input.MinCloseReasonIdFilter)
                        .WhereIf(input.MaxCloseReasonIdFilter != null, e => e.CloseReasonId <= input.MaxCloseReasonIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NoteFilter), e => e.Note.Contains(input.NoteFilter));

            var query = (from o in filteredClosures
                         select new GetClosureForViewDto()
                         {
                             Closure = new ClosureDto
                             {
                                 RiskId = o.RiskId,
                                 UserId = o.UserId,
                                 ClosureDate = o.ClosureDate,
                                 CloseReasonId = o.CloseReasonId,
                                 Note = o.Note,
                                 Id = o.Id
                             }
                         });

            var closureListDtos = await query.ToListAsync();

            return _closuresExcelExporter.ExportToFile(closureListDtos);
        }

    }
}