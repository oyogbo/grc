using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.ReviewLevels.Exporting;
using CCPDemo.ReviewLevels.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;

namespace CCPDemo.ReviewLevels
{
    [AbpAuthorize(AppPermissions.Pages_Administration_ReviewLevels)]
    public class ReviewLevelsAppService : CCPDemoAppServiceBase, IReviewLevelsAppService
    {
        private readonly IRepository<ReviewLevel> _reviewLevelRepository;
        private readonly IReviewLevelsExcelExporter _reviewLevelsExcelExporter;

        public ReviewLevelsAppService(IRepository<ReviewLevel> reviewLevelRepository, IReviewLevelsExcelExporter reviewLevelsExcelExporter)
        {
            _reviewLevelRepository = reviewLevelRepository;
            _reviewLevelsExcelExporter = reviewLevelsExcelExporter;

        }

        public async Task<PagedResultDto<GetReviewLevelForViewDto>> GetAll(GetAllReviewLevelsInput input)
        {

            var filteredReviewLevels = _reviewLevelRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(input.MinValueFilter != null, e => e.Value >= input.MinValueFilter)
                        .WhereIf(input.MaxValueFilter != null, e => e.Value <= input.MaxValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredReviewLevels = filteredReviewLevels
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var reviewLevels = from o in pagedAndFilteredReviewLevels
                               select new
                               {

                                   o.Value,
                                   o.Name,
                                   Id = o.Id
                               };

            var totalCount = await filteredReviewLevels.CountAsync();

            var dbList = await reviewLevels.ToListAsync();
            var results = new List<GetReviewLevelForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetReviewLevelForViewDto()
                {
                    ReviewLevel = new ReviewLevelDto
                    {

                        Value = o.Value,
                        Name = o.Name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetReviewLevelForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetReviewLevelForViewDto> GetReviewLevelForView(int id)
        {
            var reviewLevel = await _reviewLevelRepository.GetAsync(id);

            var output = new GetReviewLevelForViewDto { ReviewLevel = ObjectMapper.Map<ReviewLevelDto>(reviewLevel) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ReviewLevels_Edit)]
        public async Task<GetReviewLevelForEditOutput> GetReviewLevelForEdit(EntityDto input)
        {
            var reviewLevel = await _reviewLevelRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetReviewLevelForEditOutput { ReviewLevel = ObjectMapper.Map<CreateOrEditReviewLevelDto>(reviewLevel) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditReviewLevelDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_ReviewLevels_Create)]
        protected virtual async Task Create(CreateOrEditReviewLevelDto input)
        {
            var reviewLevel = ObjectMapper.Map<ReviewLevel>(input);

            if (AbpSession.TenantId != null)
            {
                reviewLevel.TenantId = (int?)AbpSession.TenantId;
            }

            await _reviewLevelRepository.InsertAsync(reviewLevel);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ReviewLevels_Edit)]
        protected virtual async Task Update(CreateOrEditReviewLevelDto input)
        {
            var reviewLevel = await _reviewLevelRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, reviewLevel);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ReviewLevels_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _reviewLevelRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetReviewLevelsToExcel(GetAllReviewLevelsForExcelInput input)
        {

            var filteredReviewLevels = _reviewLevelRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(input.MinValueFilter != null, e => e.Value >= input.MinValueFilter)
                        .WhereIf(input.MaxValueFilter != null, e => e.Value <= input.MaxValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var query = (from o in filteredReviewLevels
                         select new GetReviewLevelForViewDto()
                         {
                             ReviewLevel = new ReviewLevelDto
                             {
                                 Value = o.Value,
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var reviewLevelListDtos = await query.ToListAsync();

            return _reviewLevelsExcelExporter.ExportToFile(reviewLevelListDtos);
        }

    }
}