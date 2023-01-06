using System.Collections.Generic;
using CCPDemo.RiskTypes.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.RiskTypes.Exporting
{
    public interface IRiskTypesExcelExporter
    {
        FileDto ExportToFile(List<GetRiskTypeForViewDto> riskTypes);
    }
}