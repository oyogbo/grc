using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.RiskGroupings.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.RiskGroupings
{
    public interface IRiskGroupingsAppService : IApplicationService
    {
        Task<PagedResultDto<GetRiskGroupingForViewDto>> GetAll(GetAllRiskGroupingsInput input);

        Task<GetRiskGroupingForViewDto> GetRiskGroupingForView(int id);

        Task<GetRiskGroupingForEditOutput> GetRiskGroupingForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditRiskGroupingDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetRiskGroupingsToExcel(GetAllRiskGroupingsForExcelInput input);

    }
}