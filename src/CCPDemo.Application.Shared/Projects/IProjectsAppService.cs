using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.Projects.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.Projects
{
    public interface IProjectsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProjectForViewDto>> GetAll(GetAllProjectsInput input);

        Task<GetProjectForViewDto> GetProjectForView(int id);

        Task<GetProjectForEditOutput> GetProjectForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditProjectDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetProjectsToExcel(GetAllProjectsForExcelInput input);

    }
}