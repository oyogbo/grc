using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.RiskRatings.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.RiskRatings
{
    public interface IRiskRatingsAppService : IApplicationService
    {
        Task<PagedResultDto<GetRiskRatingForViewDto>> GetAll(GetAllRiskRatingsInput input);

        Task<GetRiskRatingForViewDto> GetRiskRatingForView(int id);

        Task<GetRiskRatingForEditOutput> GetRiskRatingForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditRiskRatingDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetRiskRatingsToExcel(GetAllRiskRatingsForExcelInput input);

    }
}