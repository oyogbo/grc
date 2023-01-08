using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.RiskStatuses.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.RiskStatuses
{
    public interface IRiskStatusAppService : IApplicationService
    {
        Task<PagedResultDto<GetRiskStatusForViewDto>> GetAll(GetAllRiskStatusInput input);

        Task<GetRiskStatusForViewDto> GetRiskStatusForView(int id);

        Task<GetRiskStatusForEditOutput> GetRiskStatusForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditRiskStatusDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetRiskStatusToExcel(GetAllRiskStatusForExcelInput input);

    }
}