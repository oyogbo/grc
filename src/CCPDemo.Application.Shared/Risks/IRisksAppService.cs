using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.Risks.Dtos;
using CCPDemo.Dto;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;
using CCPDemo.RiskTransactions.Dtos;
using CCPDemo.RiskRatings.Dtos;
using System.Collections;
using System.Linq;

namespace CCPDemo.Risks
{
    public interface IRisksAppService : IApplicationService
    {
        Task<PagedResultDto<GetRiskForViewDto>> GetAll(GetAllRisksInput input);
        Task<List<GetRiskForViewDto>> GetRisks();

        Task<GetRiskForViewDto> GetRiskForView(int id);

        Task<GetRiskForEditOutput> GetRiskForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditRiskDto input);
        string GetOrganizationalUnitName(int? id);
        Task TransferRisk(CreateOrEditRiskRiskTransactionDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetRisksToExcel(GetAllRisksForExcelInput input);

        Task<List<RiskRiskTypeLookupTableDto>> GetAllRiskTypeForTableDropdown();

        Task<List<RiskOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForTableDropdown();

        Task<List<RiskStatusLookupTableDto>> GetAllStatusForTableDropdown();

        Task<List<RiskRiskRatingLookupTableDto>> GetAllRiskRatingForTableDropdown();

        Task<List<RiskUserLookupTableDto>> GetAllUserForTableDropdown();
        Task<PagedResultDto<GetRiskForViewDto>> ClosedRisks(GetAllRisksInput input);
        Task<PagedResultDto<GetRiskForViewDto>> OnGoingRisks(GetAllRisksInput input);
        Task<PagedResultDto<GetRiskForViewDto>> FilteredRisks(GetAllRisksInput input);
        Task<PagedResultDto<GetRiskForViewDto>> OverDueRisks(GetAllRisksInput input);
        Task<PagedResultDto<RiskTypeByDepartment>> GetRiskTypeByDepartment();

    }
}