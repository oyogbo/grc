using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.Employee.Exporting;
using CCPDemo.Employee.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;

namespace CCPDemo.Employee
{
    [AbpAuthorize(AppPermissions.Pages_Employees)]
    public class EmployeesAppService : CCPDemoAppServiceBase, IEmployeesAppService
    {
        private readonly IRepository<Employees> _employeesRepository;
        private readonly IEmployeesExcelExporter _employeesExcelExporter;

        public EmployeesAppService(IRepository<Employees> employeesRepository, IEmployeesExcelExporter employeesExcelExporter)
        {
            _employeesRepository = employeesRepository;
            _employeesExcelExporter = employeesExcelExporter;

        }

        public async Task<PagedResultDto<GetEmployeesForViewDto>> GetAll(GetAllEmployeesInput input)
        {

            var filteredEmployees = _employeesRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LastName.Contains(input.Filter) || e.FirstName.Contains(input.Filter) || e.Title.Contains(input.Filter) || e.TitleOfCourtesy.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.City.Contains(input.Filter) || e.Region.Contains(input.Filter) || e.PostalCode.Contains(input.Filter) || e.Country.Contains(input.Filter) || e.HomePhone.Contains(input.Filter) || e.Extension.Contains(input.Filter) || e.Photo.Contains(input.Filter) || e.Notes.Contains(input.Filter) || e.PhotoPath.Contains(input.Filter))
                        .WhereIf(input.MinEmployeeIDFilter != null, e => e.EmployeeID >= input.MinEmployeeIDFilter)
                        .WhereIf(input.MaxEmployeeIDFilter != null, e => e.EmployeeID <= input.MaxEmployeeIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LastNameFilter), e => e.LastName == input.LastNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameFilter), e => e.FirstName == input.FirstNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title == input.TitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleOfCourtesyFilter), e => e.TitleOfCourtesy == input.TitleOfCourtesyFilter)
                        .WhereIf(input.MinBirthDateFilter != null, e => e.BirthDate >= input.MinBirthDateFilter)
                        .WhereIf(input.MaxBirthDateFilter != null, e => e.BirthDate <= input.MaxBirthDateFilter)
                        .WhereIf(input.MinHireDateFilter != null, e => e.HireDate >= input.MinHireDateFilter)
                        .WhereIf(input.MaxHireDateFilter != null, e => e.HireDate <= input.MaxHireDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address == input.AddressFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City == input.CityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegionFilter), e => e.Region == input.RegionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PostalCodeFilter), e => e.PostalCode == input.PostalCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryFilter), e => e.Country == input.CountryFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HomePhoneFilter), e => e.HomePhone == input.HomePhoneFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExtensionFilter), e => e.Extension == input.ExtensionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhotoFilter), e => e.Photo == input.PhotoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NotesFilter), e => e.Notes == input.NotesFilter)
                        .WhereIf(input.MinReportsToFilter != null, e => e.ReportsTo >= input.MinReportsToFilter)
                        .WhereIf(input.MaxReportsToFilter != null, e => e.ReportsTo <= input.MaxReportsToFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhotoPathFilter), e => e.PhotoPath == input.PhotoPathFilter);

            var pagedAndFilteredEmployees = filteredEmployees
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var employees = from o in pagedAndFilteredEmployees
                            select new
                            {

                                o.EmployeeID,
                                o.LastName,
                                o.FirstName,
                                o.Title,
                                o.TitleOfCourtesy,
                                o.BirthDate,
                                o.HireDate,
                                o.Address,
                                o.City,
                                o.Region,
                                o.PostalCode,
                                o.Country,
                                o.HomePhone,
                                o.Extension,
                                o.Photo,
                                o.Notes,
                                o.ReportsTo,
                                o.PhotoPath,
                                Id = o.Id
                            };

            var totalCount = await filteredEmployees.CountAsync();

            var dbList = await employees.ToListAsync();
            var results = new List<GetEmployeesForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetEmployeesForViewDto()
                {
                    Employees = new EmployeesDto
                    {

                        EmployeeID = o.EmployeeID,
                        LastName = o.LastName,
                        FirstName = o.FirstName,
                        Title = o.Title,
                        TitleOfCourtesy = o.TitleOfCourtesy,
                        BirthDate = o.BirthDate,
                        HireDate = o.HireDate,
                        Address = o.Address,
                        City = o.City,
                        Region = o.Region,
                        PostalCode = o.PostalCode,
                        Country = o.Country,
                        HomePhone = o.HomePhone,
                        Extension = o.Extension,
                        Photo = o.Photo,
                        Notes = o.Notes,
                        ReportsTo = o.ReportsTo,
                        PhotoPath = o.PhotoPath,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetEmployeesForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetEmployeesForViewDto> GetEmployeesForView(int id)
        {
            var employees = await _employeesRepository.GetAsync(id);

            var output = new GetEmployeesForViewDto { Employees = ObjectMapper.Map<EmployeesDto>(employees) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Employees_Edit)]
        public async Task<GetEmployeesForEditOutput> GetEmployeesForEdit(EntityDto input)
        {
            var employees = await _employeesRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetEmployeesForEditOutput { Employees = ObjectMapper.Map<CreateOrEditEmployeesDto>(employees) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditEmployeesDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Employees_Create)]
        protected virtual async Task Create(CreateOrEditEmployeesDto input)
        {
            var employees = ObjectMapper.Map<Employees>(input);

            if (AbpSession.TenantId != null)
            {
                employees.TenantId = (int?)AbpSession.TenantId;
            }

            await _employeesRepository.InsertAsync(employees);

        }

        [AbpAuthorize(AppPermissions.Pages_Employees_Edit)]
        protected virtual async Task Update(CreateOrEditEmployeesDto input)
        {
            var employees = await _employeesRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, employees);

        }

        [AbpAuthorize(AppPermissions.Pages_Employees_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _employeesRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetEmployeesToExcel(GetAllEmployeesForExcelInput input)
        {

            var filteredEmployees = _employeesRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LastName.Contains(input.Filter) || e.FirstName.Contains(input.Filter) || e.Title.Contains(input.Filter) || e.TitleOfCourtesy.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.City.Contains(input.Filter) || e.Region.Contains(input.Filter) || e.PostalCode.Contains(input.Filter) || e.Country.Contains(input.Filter) || e.HomePhone.Contains(input.Filter) || e.Extension.Contains(input.Filter) || e.Photo.Contains(input.Filter) || e.Notes.Contains(input.Filter) || e.PhotoPath.Contains(input.Filter))
                        .WhereIf(input.MinEmployeeIDFilter != null, e => e.EmployeeID >= input.MinEmployeeIDFilter)
                        .WhereIf(input.MaxEmployeeIDFilter != null, e => e.EmployeeID <= input.MaxEmployeeIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LastNameFilter), e => e.LastName == input.LastNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameFilter), e => e.FirstName == input.FirstNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title == input.TitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleOfCourtesyFilter), e => e.TitleOfCourtesy == input.TitleOfCourtesyFilter)
                        .WhereIf(input.MinBirthDateFilter != null, e => e.BirthDate >= input.MinBirthDateFilter)
                        .WhereIf(input.MaxBirthDateFilter != null, e => e.BirthDate <= input.MaxBirthDateFilter)
                        .WhereIf(input.MinHireDateFilter != null, e => e.HireDate >= input.MinHireDateFilter)
                        .WhereIf(input.MaxHireDateFilter != null, e => e.HireDate <= input.MaxHireDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address == input.AddressFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City == input.CityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegionFilter), e => e.Region == input.RegionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PostalCodeFilter), e => e.PostalCode == input.PostalCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryFilter), e => e.Country == input.CountryFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HomePhoneFilter), e => e.HomePhone == input.HomePhoneFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExtensionFilter), e => e.Extension == input.ExtensionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhotoFilter), e => e.Photo == input.PhotoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NotesFilter), e => e.Notes == input.NotesFilter)
                        .WhereIf(input.MinReportsToFilter != null, e => e.ReportsTo >= input.MinReportsToFilter)
                        .WhereIf(input.MaxReportsToFilter != null, e => e.ReportsTo <= input.MaxReportsToFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhotoPathFilter), e => e.PhotoPath == input.PhotoPathFilter);

            var query = (from o in filteredEmployees
                         select new GetEmployeesForViewDto()
                         {
                             Employees = new EmployeesDto
                             {
                                 EmployeeID = o.EmployeeID,
                                 LastName = o.LastName,
                                 FirstName = o.FirstName,
                                 Title = o.Title,
                                 TitleOfCourtesy = o.TitleOfCourtesy,
                                 BirthDate = o.BirthDate,
                                 HireDate = o.HireDate,
                                 Address = o.Address,
                                 City = o.City,
                                 Region = o.Region,
                                 PostalCode = o.PostalCode,
                                 Country = o.Country,
                                 HomePhone = o.HomePhone,
                                 Extension = o.Extension,
                                 Photo = o.Photo,
                                 Notes = o.Notes,
                                 ReportsTo = o.ReportsTo,
                                 PhotoPath = o.PhotoPath,
                                 Id = o.Id
                             }
                         });

            var employeesListDtos = await query.ToListAsync();

            return _employeesExcelExporter.ExportToFile(employeesListDtos);
        }

    }
}