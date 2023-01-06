using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.RiskCategory.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.RiskCategory
{
    public interface ICategoryAppService : IApplicationService
    {
        Task<PagedResultDto<GetCategoryForViewDto>> GetAll(GetAllCategoryInput input);

        Task<GetCategoryForViewDto> GetCategoryForView(int id);

        Task<GetCategoryForEditOutput> GetCategoryForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCategoryDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetCategoryToExcel(GetAllCategoryForExcelInput input);

    }
}