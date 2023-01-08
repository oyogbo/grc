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

namespace CCPDemo.Risks
{
    public interface IRisksAppService : IApplicationService
    {
        Task<PagedResultDto<GetRiskForViewDto>> GetAll(GetAllRisksInput input);

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

    }
}