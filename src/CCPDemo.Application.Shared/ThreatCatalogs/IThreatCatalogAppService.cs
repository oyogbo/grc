using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.ThreatCatalogs.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.ThreatCatalogs
{
    public interface IThreatCatalogAppService : IApplicationService
    {
        Task<PagedResultDto<GetThreatCatalogForViewDto>> GetAll(GetAllThreatCatalogInput input);

        Task<GetThreatCatalogForViewDto> GetThreatCatalogForView(int id);

        Task<GetThreatCatalogForEditOutput> GetThreatCatalogForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditThreatCatalogDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetThreatCatalogToExcel(GetAllThreatCatalogForExcelInput input);

    }
}