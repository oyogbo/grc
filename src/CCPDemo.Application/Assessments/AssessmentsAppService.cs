using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.Assessments.Exporting;
using CCPDemo.Assessments.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;

namespace CCPDemo.Assessments
{
    [AbpAuthorize(AppPermissions.Pages_Assessments)]
    public class AssessmentsAppService : CCPDemoAppServiceBase, IAssessmentsAppService
    {
        private readonly IRepository<Assessment> _assessmentRepository;
        private readonly IAssessmentsExcelExporter _assessmentsExcelExporter;

        public AssessmentsAppService(IRepository<Assessment> assessmentRepository, IAssessmentsExcelExporter assessmentsExcelExporter)
        {
            _assessmentRepository = assessmentRepository;
            _assessmentsExcelExporter = assessmentsExcelExporter;

        }

        public async Task<PagedResultDto<GetAssessmentForViewDto>> GetAll(GetAllAssessmentsInput input)
        {

            var filteredAssessments = _assessmentRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredAssessments = filteredAssessments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var assessments = from o in pagedAndFilteredAssessments
                              select new
                              {

                                  o.Name,
                                  Id = o.Id
                              };

            var totalCount = await filteredAssessments.CountAsync();

            var dbList = await assessments.ToListAsync();
            var results = new List<GetAssessmentForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetAssessmentForViewDto()
                {
                    Assessment = new AssessmentDto
                    {

                        Name = o.Name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetAssessmentForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetAssessmentForViewDto> GetAssessmentForView(int id)
        {
            var assessment = await _assessmentRepository.GetAsync(id);

            var output = new GetAssessmentForViewDto { Assessment = ObjectMapper.Map<AssessmentDto>(assessment) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Assessments_Edit)]
        public async Task<GetAssessmentForEditOutput> GetAssessmentForEdit(EntityDto input)
        {
            var assessment = await _assessmentRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAssessmentForEditOutput { Assessment = ObjectMapper.Map<CreateOrEditAssessmentDto>(assessment) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAssessmentDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Assessments_Create)]
        protected virtual async Task Create(CreateOrEditAssessmentDto input)
        {
            var assessment = ObjectMapper.Map<Assessment>(input);

            if (AbpSession.TenantId != null)
            {
                assessment.TenantId = (int?)AbpSession.TenantId;
            }

            await _assessmentRepository.InsertAsync(assessment);

        }

        [AbpAuthorize(AppPermissions.Pages_Assessments_Edit)]
        protected virtual async Task Update(CreateOrEditAssessmentDto input)
        {
            var assessment = await _assessmentRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, assessment);

        }

        [AbpAuthorize(AppPermissions.Pages_Assessments_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _assessmentRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetAssessmentsToExcel(GetAllAssessmentsForExcelInput input)
        {

            var filteredAssessments = _assessmentRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var query = (from o in filteredAssessments
                         select new GetAssessmentForViewDto()
                         {
                             Assessment = new AssessmentDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var assessmentListDtos = await query.ToListAsync();

            return _assessmentsExcelExporter.ExportToFile(assessmentListDtos);
        }

    }
}