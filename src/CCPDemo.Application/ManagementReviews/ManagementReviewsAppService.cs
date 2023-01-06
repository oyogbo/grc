using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.ManagementReviews.Exporting;
using CCPDemo.ManagementReviews.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;

namespace CCPDemo.ManagementReviews
{
    [AbpAuthorize(AppPermissions.Pages_Administration_ManagementReviews)]
    public class ManagementReviewsAppService : CCPDemoAppServiceBase, IManagementReviewsAppService
    {
        private readonly IRepository<ManagementReview> _managementReviewRepository;
        private readonly IManagementReviewsExcelExporter _managementReviewsExcelExporter;

        public ManagementReviewsAppService(IRepository<ManagementReview> managementReviewRepository, IManagementReviewsExcelExporter managementReviewsExcelExporter)
        {
            _managementReviewRepository = managementReviewRepository;
            _managementReviewsExcelExporter = managementReviewsExcelExporter;

        }

        public async Task<PagedResultDto<GetManagementReviewForViewDto>> GetAll(GetAllManagementReviewsInput input)
        {

            var filteredManagementReviews = _managementReviewRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.comments.Contains(input.Filter))
                        .WhereIf(input.Minrisk_idFilter != null, e => e.risk_id >= input.Minrisk_idFilter)
                        .WhereIf(input.Maxrisk_idFilter != null, e => e.risk_id <= input.Maxrisk_idFilter)
                        .WhereIf(input.Minsubmission_dateFilter != null, e => e.submission_date >= input.Minsubmission_dateFilter)
                        .WhereIf(input.Maxsubmission_dateFilter != null, e => e.submission_date <= input.Maxsubmission_dateFilter)
                        .WhereIf(input.MinreviewFilter != null, e => e.review >= input.MinreviewFilter)
                        .WhereIf(input.MaxreviewFilter != null, e => e.review <= input.MaxreviewFilter)
                        .WhereIf(input.MinreviewerFilter != null, e => e.reviewer >= input.MinreviewerFilter)
                        .WhereIf(input.MaxreviewerFilter != null, e => e.reviewer <= input.MaxreviewerFilter)
                        .WhereIf(input.Minnext_stepFilter != null, e => e.next_step >= input.Minnext_stepFilter)
                        .WhereIf(input.Maxnext_stepFilter != null, e => e.next_step <= input.Maxnext_stepFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.commentsFilter), e => e.comments == input.commentsFilter)
                        .WhereIf(input.Minnext_reviewFilter != null, e => e.next_review >= input.Minnext_reviewFilter)
                        .WhereIf(input.Maxnext_reviewFilter != null, e => e.next_review <= input.Maxnext_reviewFilter);

            var pagedAndFilteredManagementReviews = filteredManagementReviews
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var managementReviews = from o in pagedAndFilteredManagementReviews
                                    select new
                                    {

                                        o.risk_id,
                                        o.submission_date,
                                        o.review,
                                        o.reviewer,
                                        o.next_step,
                                        o.comments,
                                        o.next_review,
                                        Id = o.Id
                                    };

            var totalCount = await filteredManagementReviews.CountAsync();

            var dbList = await managementReviews.ToListAsync();
            var results = new List<GetManagementReviewForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetManagementReviewForViewDto()
                {
                    ManagementReview = new ManagementReviewDto
                    {

                        risk_id = o.risk_id,
                        submission_date = o.submission_date,
                        review = o.review,
                        reviewer = o.reviewer,
                        next_step = o.next_step,
                        comments = o.comments,
                        next_review = o.next_review,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetManagementReviewForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetManagementReviewForViewDto> GetManagementReviewForView(int id)
        {
            var managementReview = await _managementReviewRepository.GetAsync(id);

            var output = new GetManagementReviewForViewDto { ManagementReview = ObjectMapper.Map<ManagementReviewDto>(managementReview) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ManagementReviews_Edit)]
        public async Task<GetManagementReviewForEditOutput> GetManagementReviewForEdit(EntityDto input)
        {
            var managementReview = await _managementReviewRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetManagementReviewForEditOutput { ManagementReview = ObjectMapper.Map<CreateOrEditManagementReviewDto>(managementReview) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditManagementReviewDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_ManagementReviews_Create)]
        protected virtual async Task Create(CreateOrEditManagementReviewDto input)
        {
            var managementReview = ObjectMapper.Map<ManagementReview>(input);

            if (AbpSession.TenantId != null)
            {
                managementReview.TenantId = (int?)AbpSession.TenantId;
            }

            await _managementReviewRepository.InsertAsync(managementReview);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ManagementReviews_Edit)]
        protected virtual async Task Update(CreateOrEditManagementReviewDto input)
        {
            var managementReview = await _managementReviewRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, managementReview);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ManagementReviews_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _managementReviewRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetManagementReviewsToExcel(GetAllManagementReviewsForExcelInput input)
        {

            var filteredManagementReviews = _managementReviewRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.comments.Contains(input.Filter))
                        .WhereIf(input.Minrisk_idFilter != null, e => e.risk_id >= input.Minrisk_idFilter)
                        .WhereIf(input.Maxrisk_idFilter != null, e => e.risk_id <= input.Maxrisk_idFilter)
                        .WhereIf(input.Minsubmission_dateFilter != null, e => e.submission_date >= input.Minsubmission_dateFilter)
                        .WhereIf(input.Maxsubmission_dateFilter != null, e => e.submission_date <= input.Maxsubmission_dateFilter)
                        .WhereIf(input.MinreviewFilter != null, e => e.review >= input.MinreviewFilter)
                        .WhereIf(input.MaxreviewFilter != null, e => e.review <= input.MaxreviewFilter)
                        .WhereIf(input.MinreviewerFilter != null, e => e.reviewer >= input.MinreviewerFilter)
                        .WhereIf(input.MaxreviewerFilter != null, e => e.reviewer <= input.MaxreviewerFilter)
                        .WhereIf(input.Minnext_stepFilter != null, e => e.next_step >= input.Minnext_stepFilter)
                        .WhereIf(input.Maxnext_stepFilter != null, e => e.next_step <= input.Maxnext_stepFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.commentsFilter), e => e.comments == input.commentsFilter)
                        .WhereIf(input.Minnext_reviewFilter != null, e => e.next_review >= input.Minnext_reviewFilter)
                        .WhereIf(input.Maxnext_reviewFilter != null, e => e.next_review <= input.Maxnext_reviewFilter);

            var query = (from o in filteredManagementReviews
                         select new GetManagementReviewForViewDto()
                         {
                             ManagementReview = new ManagementReviewDto
                             {
                                 risk_id = o.risk_id,
                                 submission_date = o.submission_date,
                                 review = o.review,
                                 reviewer = o.reviewer,
                                 next_step = o.next_step,
                                 comments = o.comments,
                                 next_review = o.next_review,
                                 Id = o.Id
                             }
                         });

            var managementReviewListDtos = await query.ToListAsync();

            return _managementReviewsExcelExporter.ExportToFile(managementReviewListDtos);
        }

    }
}