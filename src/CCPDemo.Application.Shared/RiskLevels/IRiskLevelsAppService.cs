using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.RiskLevels.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.RiskLevels
{
    public interface IRiskLevelsAppService : IApplicationService
    {
        Task<PagedResultDto<GetRiskLevelForViewDto>> GetAll(GetAllRiskLevelsInput input);

        Task<GetRiskLevelForViewDto> GetRiskLevelForView(int id);

        Task<GetRiskLevelForEditOutput> GetRiskLevelForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditRiskLevelDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetRiskLevelsToExcel(GetAllRiskLevelsForExcelInput input);

    }
}