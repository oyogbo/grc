using System.Collections.Generic;
using CCPDemo.RiskLevels.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.RiskLevels.Exporting
{
    public interface IRiskLevelsExcelExporter
    {
        FileDto ExportToFile(List<GetRiskLevelForViewDto> riskLevels);
    }
}