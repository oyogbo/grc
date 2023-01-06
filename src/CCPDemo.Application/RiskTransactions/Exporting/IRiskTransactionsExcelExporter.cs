using System.Collections.Generic;
using CCPDemo.RiskTransactions.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.RiskTransactions.Exporting
{
    public interface IRiskTransactionsExcelExporter
    {
        FileDto ExportToFile(List<GetRiskTransactionForViewDto> riskTransactions);
    }
}