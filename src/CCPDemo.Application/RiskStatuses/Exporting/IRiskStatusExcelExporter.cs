using System.Collections.Generic;
using CCPDemo.RiskStatuses.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.RiskStatuses.Exporting
{
    public interface IRiskStatusExcelExporter
    {
        FileDto ExportToFile(List<GetRiskStatusForViewDto> riskStatus);
    }
}