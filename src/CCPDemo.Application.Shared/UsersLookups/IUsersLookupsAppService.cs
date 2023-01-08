using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.UsersLookups.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.UsersLookups
{
    public interface IUsersLookupsAppService : IApplicationService
    {
        Task<PagedResultDto<GetUsersLookupForViewDto>> GetAll(GetAllUsersLookupsInput input);

        Task<GetUsersLookupForViewDto> GetUsersLookupForView(int id);

        Task<GetUsersLookupForEditOutput> GetUsersLookupForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditUsersLookupDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetUsersLookupsToExcel(GetAllUsersLookupsForExcelInput input);

        Task<PagedResultDto<UsersLookupUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

    }
}