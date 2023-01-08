using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.RiskRatings.Exporting;
using CCPDemo.RiskRatings.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;

namespace CCPDemo.RiskRatings
{
    [AbpAuthorize(AppPermissions.Pages_Administration_RiskRatings)]
    public class RiskRatingsAppService : CCPDemoAppServiceBase, IRiskRatingsAppService
    {
        private readonly IRepository<RiskRating> _riskRatingRepository;
        private readonly IRiskRatingsExcelExporter _riskRatingsExcelExporter;

        public RiskRatingsAppService(IRepository<RiskRating> riskRatingRepository, IRiskRatingsExcelExporter riskRatingsExcelExporter)
        {
            _riskRatingRepository = riskRatingRepository;
            _riskRatingsExcelExporter = riskRatingsExcelExporter;

        }

        public async Task<PagedResultDto<GetRiskRatingForViewDto>> GetAll(GetAllRiskRatingsInput input)
        {

            var filteredRiskRatings = _riskRatingRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Color.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ColorFilter), e => e.Color == input.ColorFilter);

            var pagedAndFilteredRiskRatings = filteredRiskRatings
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var riskRatings = from o in pagedAndFilteredRiskRatings
                              select new
                              {

                                  o.Name,
                                  o.Color,
                                  Id = o.Id
                              };

            var totalCount = await filteredRiskRatings.CountAsync();

            var dbList = await riskRatings.ToListAsync();
            var results = new List<GetRiskRatingForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetRiskRatingForViewDto()
                {
                    RiskRating = new RiskRatingDto
                    {

                        Name = o.Name,
                        Color = o.Color,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetRiskRatingForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetRiskRatingForViewDto> GetRiskRatingForView(int id)
        {
            var riskRating = await _riskRatingRepository.GetAsync(id);

            var output = new GetRiskRatingForViewDto { RiskRating = ObjectMapper.Map<RiskRatingDto>(riskRating) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskRatings_Edit)]
        public async Task<GetRiskRatingForEditOutput> GetRiskRatingForEdit(EntityDto input)
        {
            var riskRating = await _riskRatingRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRiskRatingForEditOutput { RiskRating = ObjectMapper.Map<CreateOrEditRiskRatingDto>(riskRating) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditRiskRatingDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskRatings_Create)]
        protected virtual async Task Create(CreateOrEditRiskRatingDto input)
        {
            var riskRating = ObjectMapper.Map<RiskRating>(input);

            await _riskRatingRepository.InsertAsync(riskRating);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskRatings_Edit)]
        protected virtual async Task Update(CreateOrEditRiskRatingDto input)
        {
            var riskRating = await _riskRatingRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, riskRating);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RiskRatings_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _riskRatingRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetRiskRatingsToExcel(GetAllRiskRatingsForExcelInput input)
        {

            var filteredRiskRatings = _riskRatingRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Color.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ColorFilter), e => e.Color == input.ColorFilter);

            var query = (from o in filteredRiskRatings
                         select new GetRiskRatingForViewDto()
                         {
                             RiskRating = new RiskRatingDto
                             {
                                 Name = o.Name,
                                 Color = o.Color,
                                 Id = o.Id
                             }
                         });

            var riskRatingListDtos = await query.ToListAsync();

            return _riskRatingsExcelExporter.ExportToFile(riskRatingListDtos);
        }

    }
}