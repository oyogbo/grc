using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.Regulations.Exporting;
using CCPDemo.Regulations.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;

namespace CCPDemo.Regulations
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Regulations)]
    public class RegulationsAppService : CCPDemoAppServiceBase, IRegulationsAppService
    {
        private readonly IRepository<Regulation> _regulationRepository;
        private readonly IRegulationsExcelExporter _regulationsExcelExporter;

        public RegulationsAppService(IRepository<Regulation> regulationRepository, IRegulationsExcelExporter regulationsExcelExporter)
        {
            _regulationRepository = regulationRepository;
            _regulationsExcelExporter = regulationsExcelExporter;

        }

        public async Task<PagedResultDto<GetRegulationForViewDto>> GetAll(GetAllRegulationsInput input)
        {

            var filteredRegulations = _regulationRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.name.Contains(input.Filter))
                        .WhereIf(input.MinvalueFilter != null, e => e.value >= input.MinvalueFilter)
                        .WhereIf(input.MaxvalueFilter != null, e => e.value <= input.MaxvalueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.nameFilter), e => e.name == input.nameFilter);

            var pagedAndFilteredRegulations = filteredRegulations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var regulations = from o in pagedAndFilteredRegulations
                              select new
                              {

                                  o.value,
                                  o.name,
                                  Id = o.Id
                              };

            var totalCount = await filteredRegulations.CountAsync();

            var dbList = await regulations.ToListAsync();
            var results = new List<GetRegulationForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetRegulationForViewDto()
                {
                    Regulation = new RegulationDto
                    {

                        value = o.value,
                        name = o.name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetRegulationForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetRegulationForViewDto> GetRegulationForView(int id)
        {
            var regulation = await _regulationRepository.GetAsync(id);

            var output = new GetRegulationForViewDto { Regulation = ObjectMapper.Map<RegulationDto>(regulation) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Regulations_Edit)]
        public async Task<GetRegulationForEditOutput> GetRegulationForEdit(EntityDto input)
        {
            var regulation = await _regulationRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRegulationForEditOutput { Regulation = ObjectMapper.Map<CreateOrEditRegulationDto>(regulation) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditRegulationDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_Regulations_Create)]
        protected virtual async Task Create(CreateOrEditRegulationDto input)
        {
            var regulation = ObjectMapper.Map<Regulation>(input);

            if (AbpSession.TenantId != null)
            {
                regulation.TenantId = (int?)AbpSession.TenantId;
            }

            await _regulationRepository.InsertAsync(regulation);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Regulations_Edit)]
        protected virtual async Task Update(CreateOrEditRegulationDto input)
        {
            var regulation = await _regulationRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, regulation);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Regulations_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _regulationRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetRegulationsToExcel(GetAllRegulationsForExcelInput input)
        {

            var filteredRegulations = _regulationRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.name.Contains(input.Filter))
                        .WhereIf(input.MinvalueFilter != null, e => e.value >= input.MinvalueFilter)
                        .WhereIf(input.MaxvalueFilter != null, e => e.value <= input.MaxvalueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.nameFilter), e => e.name == input.nameFilter);

            var query = (from o in filteredRegulations
                         select new GetRegulationForViewDto()
                         {
                             Regulation = new RegulationDto
                             {
                                 value = o.value,
                                 name = o.name,
                                 Id = o.Id
                             }
                         });

            var regulationListDtos = await query.ToListAsync();

            return _regulationsExcelExporter.ExportToFile(regulationListDtos);
        }

    }
}