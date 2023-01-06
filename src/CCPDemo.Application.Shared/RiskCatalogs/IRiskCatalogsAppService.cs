using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.RiskCatalogs.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.RiskCatalogs
{
    public interface IRiskCatalogsAppService : IApplicationService
    {
        Task<PagedResultDto<GetRiskCatalogForViewDto>> GetAll(GetAllRiskCatalogsInput input);

        Task<GetRiskCatalogForViewDto> GetRiskCatalogForView(int id);

        Task<GetRiskCatalogForEditOutput> GetRiskCatalogForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditRiskCatalogDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetRiskCatalogsToExcel(GetAllRiskCatalogsForExcelInput input);

    }
}