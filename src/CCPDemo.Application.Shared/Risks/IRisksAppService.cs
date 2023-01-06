using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.Risks.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.Risks
{
    public interface IRisksAppService : IApplicationService
    {
        Task<PagedResultDto<GetRiskForViewDto>> GetAll(GetAllRisksInput input);

        Task<GetRiskForViewDto> GetRiskForView(int id);

        Task<GetRiskForEditOutput> GetRiskForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditRiskDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetRisksToExcel(GetAllRisksForExcelInput input);

    }
}