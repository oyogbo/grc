using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.ReviewLevels.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.ReviewLevels
{
    public interface IReviewLevelsAppService : IApplicationService
    {
        Task<PagedResultDto<GetReviewLevelForViewDto>> GetAll(GetAllReviewLevelsInput input);

        Task<GetReviewLevelForViewDto> GetReviewLevelForView(int id);

        Task<GetReviewLevelForEditOutput> GetReviewLevelForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditReviewLevelDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetReviewLevelsToExcel(GetAllReviewLevelsForExcelInput input);

    }
}