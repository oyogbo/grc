using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.RiskModels.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.RiskModels
{
    public interface IRiskModelsAppService : IApplicationService
    {
        Task<PagedResultDto<GetRiskModelForViewDto>> GetAll(GetAllRiskModelsInput input);

        Task<GetRiskModelForViewDto> GetRiskModelForView(int id);

        Task<GetRiskModelForEditOutput> GetRiskModelForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditRiskModelDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetRiskModelsToExcel(GetAllRiskModelsForExcelInput input);

    }
}