using System.Collections.Generic;
using CCPDemo.RiskGroupings.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.RiskGroupings.Exporting
{
    public interface IRiskGroupingsExcelExporter
    {
        FileDto ExportToFile(List<GetRiskGroupingForViewDto> riskGroupings);
    }
}