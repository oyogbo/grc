using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.RiskFunctions.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.RiskFunctions
{
    public interface IRiskFunctionsAppService : IApplicationService
    {
        Task<PagedResultDto<GetRiskFunctionForViewDto>> GetAll(GetAllRiskFunctionsInput input);

        Task<GetRiskFunctionForViewDto> GetRiskFunctionForView(int id);

        Task<GetRiskFunctionForEditOutput> GetRiskFunctionForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditRiskFunctionDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetRiskFunctionsToExcel(GetAllRiskFunctionsForExcelInput input);

    }
}