using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.RiskCloseReason.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.RiskCloseReason
{
    public interface ICloseReasonsAppService : IApplicationService
    {
        Task<PagedResultDto<GetCloseReasonForViewDto>> GetAll(GetAllCloseReasonsInput input);

        Task<GetCloseReasonForViewDto> GetCloseReasonForView(int id);

        Task<GetCloseReasonForEditOutput> GetCloseReasonForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCloseReasonDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetCloseReasonsToExcel(GetAllCloseReasonsForExcelInput input);

    }
}