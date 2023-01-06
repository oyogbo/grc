using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.Regulations.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.Regulations
{
    public interface IRegulationsAppService : IApplicationService
    {
        Task<PagedResultDto<GetRegulationForViewDto>> GetAll(GetAllRegulationsInput input);

        Task<GetRegulationForViewDto> GetRegulationForView(int id);

        Task<GetRegulationForEditOutput> GetRegulationForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditRegulationDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetRegulationsToExcel(GetAllRegulationsForExcelInput input);

    }
}