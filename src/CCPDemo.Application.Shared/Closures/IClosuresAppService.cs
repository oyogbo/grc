using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.Closures.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.Closures
{
    public interface IClosuresAppService : IApplicationService
    {
        Task<PagedResultDto<GetClosureForViewDto>> GetAll(GetAllClosuresInput input);

        Task<GetClosureForViewDto> GetClosureForView(int id);

        Task<GetClosureForEditOutput> GetClosureForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditClosureDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetClosuresToExcel(GetAllClosuresForExcelInput input);

    }
}