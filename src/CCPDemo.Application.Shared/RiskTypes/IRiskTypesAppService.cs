using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.RiskTypes.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.RiskTypes
{
    public interface IRiskTypesAppService : IApplicationService
    {
        Task<PagedResultDto<GetRiskTypeForViewDto>> GetAll(GetAllRiskTypesInput input);

        Task<GetRiskTypeForViewDto> GetRiskTypeForView(int id);

        Task<GetRiskTypeForEditOutput> GetRiskTypeForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditRiskTypeDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetRiskTypesToExcel(GetAllRiskTypesForExcelInput input);

    }
}