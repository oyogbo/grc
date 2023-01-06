using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.Projects.Exporting;
using CCPDemo.Projects.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;

namespace CCPDemo.Projects
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Projects)]
    public class ProjectsAppService : CCPDemoAppServiceBase, IProjectsAppService
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IProjectsExcelExporter _projectsExcelExporter;

        public ProjectsAppService(IRepository<Project> projectRepository, IProjectsExcelExporter projectsExcelExporter)
        {
            _projectRepository = projectRepository;
            _projectsExcelExporter = projectsExcelExporter;

        }

        public async Task<PagedResultDto<GetProjectForViewDto>> GetAll(GetAllProjectsInput input)
        {

            var filteredProjects = _projectRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(input.MinValueFilter != null, e => e.Value >= input.MinValueFilter)
                        .WhereIf(input.MaxValueFilter != null, e => e.Value <= input.MaxValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinDueDateFilter != null, e => e.DueDate >= input.MinDueDateFilter)
                        .WhereIf(input.MaxDueDateFilter != null, e => e.DueDate <= input.MaxDueDateFilter)
                        .WhereIf(input.MinConsultantIdFilter != null, e => e.ConsultantId >= input.MinConsultantIdFilter)
                        .WhereIf(input.MaxConsultantIdFilter != null, e => e.ConsultantId <= input.MaxConsultantIdFilter)
                        .WhereIf(input.MinBusinessOwnerIdFilter != null, e => e.BusinessOwnerId >= input.MinBusinessOwnerIdFilter)
                        .WhereIf(input.MaxBusinessOwnerIdFilter != null, e => e.BusinessOwnerId <= input.MaxBusinessOwnerIdFilter)
                        .WhereIf(input.MinDataClassificationIdFilter != null, e => e.DataClassificationId >= input.MinDataClassificationIdFilter)
                        .WhereIf(input.MaxDataClassificationIdFilter != null, e => e.DataClassificationId <= input.MaxDataClassificationIdFilter)
                        .WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
                        .WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter)
                        .WhereIf(input.MinstatusFilter != null, e => e.status >= input.MinstatusFilter)
                        .WhereIf(input.MaxstatusFilter != null, e => e.status <= input.MaxstatusFilter);

            var pagedAndFilteredProjects = filteredProjects
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var projects = from o in pagedAndFilteredProjects
                           select new
                           {

                               o.Value,
                               o.Name,
                               o.DueDate,
                               o.ConsultantId,
                               o.BusinessOwnerId,
                               o.DataClassificationId,
                               o.Order,
                               o.status,
                               Id = o.Id
                           };

            var totalCount = await filteredProjects.CountAsync();

            var dbList = await projects.ToListAsync();
            var results = new List<GetProjectForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProjectForViewDto()
                {
                    Project = new ProjectDto
                    {

                        Value = o.Value,
                        Name = o.Name,
                        DueDate = o.DueDate,
                        ConsultantId = o.ConsultantId,
                        BusinessOwnerId = o.BusinessOwnerId,
                        DataClassificationId = o.DataClassificationId,
                        Order = o.Order,
                        status = o.status,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProjectForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProjectForViewDto> GetProjectForView(int id)
        {
            var project = await _projectRepository.GetAsync(id);

            var output = new GetProjectForViewDto { Project = ObjectMapper.Map<ProjectDto>(project) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Projects_Edit)]
        public async Task<GetProjectForEditOutput> GetProjectForEdit(EntityDto input)
        {
            var project = await _projectRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProjectForEditOutput { Project = ObjectMapper.Map<CreateOrEditProjectDto>(project) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProjectDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_Projects_Create)]
        protected virtual async Task Create(CreateOrEditProjectDto input)
        {
            var project = ObjectMapper.Map<Project>(input);

            if (AbpSession.TenantId != null)
            {
                project.TenantId = (int?)AbpSession.TenantId;
            }

            await _projectRepository.InsertAsync(project);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Projects_Edit)]
        protected virtual async Task Update(CreateOrEditProjectDto input)
        {
            var project = await _projectRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, project);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Projects_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _projectRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProjectsToExcel(GetAllProjectsForExcelInput input)
        {

            var filteredProjects = _projectRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(input.MinValueFilter != null, e => e.Value >= input.MinValueFilter)
                        .WhereIf(input.MaxValueFilter != null, e => e.Value <= input.MaxValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinDueDateFilter != null, e => e.DueDate >= input.MinDueDateFilter)
                        .WhereIf(input.MaxDueDateFilter != null, e => e.DueDate <= input.MaxDueDateFilter)
                        .WhereIf(input.MinConsultantIdFilter != null, e => e.ConsultantId >= input.MinConsultantIdFilter)
                        .WhereIf(input.MaxConsultantIdFilter != null, e => e.ConsultantId <= input.MaxConsultantIdFilter)
                        .WhereIf(input.MinBusinessOwnerIdFilter != null, e => e.BusinessOwnerId >= input.MinBusinessOwnerIdFilter)
                        .WhereIf(input.MaxBusinessOwnerIdFilter != null, e => e.BusinessOwnerId <= input.MaxBusinessOwnerIdFilter)
                        .WhereIf(input.MinDataClassificationIdFilter != null, e => e.DataClassificationId >= input.MinDataClassificationIdFilter)
                        .WhereIf(input.MaxDataClassificationIdFilter != null, e => e.DataClassificationId <= input.MaxDataClassificationIdFilter)
                        .WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
                        .WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter)
                        .WhereIf(input.MinstatusFilter != null, e => e.status >= input.MinstatusFilter)
                        .WhereIf(input.MaxstatusFilter != null, e => e.status <= input.MaxstatusFilter);

            var query = (from o in filteredProjects
                         select new GetProjectForViewDto()
                         {
                             Project = new ProjectDto
                             {
                                 Value = o.Value,
                                 Name = o.Name,
                                 DueDate = o.DueDate,
                                 ConsultantId = o.ConsultantId,
                                 BusinessOwnerId = o.BusinessOwnerId,
                                 DataClassificationId = o.DataClassificationId,
                                 Order = o.Order,
                                 status = o.status,
                                 Id = o.Id
                             }
                         });

            var projectListDtos = await query.ToListAsync();

            return _projectsExcelExporter.ExportToFile(projectListDtos);
        }

    }
}