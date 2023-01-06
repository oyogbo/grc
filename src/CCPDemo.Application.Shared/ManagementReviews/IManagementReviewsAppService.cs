using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.ManagementReviews.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.ManagementReviews
{
    public interface IManagementReviewsAppService : IApplicationService
    {
        Task<PagedResultDto<GetManagementReviewForViewDto>> GetAll(GetAllManagementReviewsInput input);

        Task<GetManagementReviewForViewDto> GetManagementReviewForView(int id);

        Task<GetManagementReviewForEditOutput> GetManagementReviewForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditManagementReviewDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetManagementReviewsToExcel(GetAllManagementReviewsForExcelInput input);

    }
}