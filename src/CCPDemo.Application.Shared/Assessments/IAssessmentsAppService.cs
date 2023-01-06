using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.Assessments.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.Assessments
{
    public interface IAssessmentsAppService : IApplicationService
    {
        Task<PagedResultDto<GetAssessmentForViewDto>> GetAll(GetAllAssessmentsInput input);

        Task<GetAssessmentForViewDto> GetAssessmentForView(int id);

        Task<GetAssessmentForEditOutput> GetAssessmentForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditAssessmentDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetAssessmentsToExcel(GetAllAssessmentsForExcelInput input);

    }
}