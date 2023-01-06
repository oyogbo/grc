using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.VRisks.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.VRisks
{
    public interface IVRisksAppService : IApplicationService
    {
        Task<PagedResultDto<GetVRiskForViewDto>> GetAll(GetAllVRisksInput input);

        Task<GetVRiskForViewDto> GetVRiskForView(int id);

        Task<GetVRiskForEditOutput> GetVRiskForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditVRiskDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetVRisksToExcel(GetAllVRisksForExcelInput input);

    }
}