using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.RiskTransactions.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.RiskTransactions
{
    public interface IRiskTransactionsAppService : IApplicationService
    {
        Task<PagedResultDto<GetRiskTransactionForViewDto>> GetAll(GetAllRiskTransactionsInput input);

        Task<GetRiskTransactionForViewDto> GetRiskTransactionForView(int id);

        Task<GetRiskTransactionForEditOutput> GetRiskTransactionForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditRiskTransactionDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetRiskTransactionsToExcel(GetAllRiskTransactionsForExcelInput input);

    }
}