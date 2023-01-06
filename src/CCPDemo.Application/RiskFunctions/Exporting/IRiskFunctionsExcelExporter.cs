using System.Collections.Generic;
using CCPDemo.RiskFunctions.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.RiskFunctions.Exporting
{
    public interface IRiskFunctionsExcelExporter
    {
        FileDto ExportToFile(List<GetRiskFunctionForViewDto> riskFunctions);
    }
}