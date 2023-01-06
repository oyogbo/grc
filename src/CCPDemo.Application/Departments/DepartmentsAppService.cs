using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.Departments.Exporting;
using CCPDemo.Departments.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;

namespace CCPDemo.Departments
{
    [AbpAuthorize(AppPermissions.Pages_Departments)]
    public class DepartmentsAppService : CCPDemoAppServiceBase, IDepartmentsAppService
    {
        private readonly IRepository<Department> _departmentRepository;
        private readonly IDepartmentsExcelExporter _departmentsExcelExporter;

        public DepartmentsAppService(IRepository<Department> departmentRepository, IDepartmentsExcelExporter departmentsExcelExporter)
        {
            _departmentRepository = departmentRepository;
            _departmentsExcelExporter = departmentsExcelExporter;

        }

        public async Task<PagedResultDto<GetDepartmentForViewDto>> GetAll(GetAllDepartmentsInput input)
        {

            var filteredDepartments = _departmentRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var pagedAndFilteredDepartments = filteredDepartments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var departments = from o in pagedAndFilteredDepartments
                              select new
                              {

                                  o.Name,
                                  Id = o.Id
                              };

            var totalCount = await filteredDepartments.CountAsync();

            var dbList = await departments.ToListAsync();
            var results = new List<GetDepartmentForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDepartmentForViewDto()
                {
                    Department = new DepartmentDto
                    {

                        Name = o.Name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDepartmentForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetDepartmentForViewDto> GetDepartmentForView(int id)
        {
            var department = await _departmentRepository.GetAsync(id);

            var output = new GetDepartmentForViewDto { Department = ObjectMapper.Map<DepartmentDto>(department) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Departments_Edit)]
        public async Task<GetDepartmentForEditOutput> GetDepartmentForEdit(EntityDto input)
        {
            var department = await _departmentRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDepartmentForEditOutput { Department = ObjectMapper.Map<CreateOrEditDepartmentDto>(department) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDepartmentDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Departments_Create)]
        protected virtual async Task Create(CreateOrEditDepartmentDto input)
        {
            var department = ObjectMapper.Map<Department>(input);

            if (AbpSession.TenantId != null)
            {
                department.TenantId = (int?)AbpSession.TenantId;
            }

            await _departmentRepository.InsertAsync(department);

        }

        [AbpAuthorize(AppPermissions.Pages_Departments_Edit)]
        protected virtual async Task Update(CreateOrEditDepartmentDto input)
        {
            var department = await _departmentRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, department);

        }

        [AbpAuthorize(AppPermissions.Pages_Departments_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _departmentRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDepartmentsToExcel(GetAllDepartmentsForExcelInput input)
        {

            var filteredDepartments = _departmentRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var query = (from o in filteredDepartments
                         select new GetDepartmentForViewDto()
                         {
                             Department = new DepartmentDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var departmentListDtos = await query.ToListAsync();

            return _departmentsExcelExporter.ExportToFile(departmentListDtos);
        }

    }
}