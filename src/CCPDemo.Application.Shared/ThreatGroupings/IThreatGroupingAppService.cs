using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.ThreatGroupings.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.ThreatGroupings
{
    public interface IThreatGroupingAppService : IApplicationService
    {
        Task<PagedResultDto<GetThreatGroupingForViewDto>> GetAll(GetAllThreatGroupingInput input);

        Task<GetThreatGroupingForViewDto> GetThreatGroupingForView(int id);

        Task<GetThreatGroupingForEditOutput> GetThreatGroupingForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditThreatGroupingDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetThreatGroupingToExcel(GetAllThreatGroupingForExcelInput input);

    }
}