using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.Employee.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.Employee
{
    public interface IEmployeesAppService : IApplicationService
    {
        Task<PagedResultDto<GetEmployeesForViewDto>> GetAll(GetAllEmployeesInput input);

        Task<GetEmployeesForViewDto> GetEmployeesForView(int id);

        Task<GetEmployeesForEditOutput> GetEmployeesForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditEmployeesDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetEmployeesToExcel(GetAllEmployeesForExcelInput input);

    }
}