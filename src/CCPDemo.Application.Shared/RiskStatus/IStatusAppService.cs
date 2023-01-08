using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.RiskStatus.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.RiskStatus
{
    public interface IStatusAppService : IApplicationService
    {
        Task<PagedResultDto<GetStatusForViewDto>> GetAll(GetAllStatusInput input);

        Task<GetStatusForViewDto> GetStatusForView(int id);

        Task<GetStatusForEditOutput> GetStatusForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditStatusDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetStatusToExcel(GetAllStatusForExcelInput input);

    }
}