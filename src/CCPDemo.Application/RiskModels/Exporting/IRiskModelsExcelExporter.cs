using System.Collections.Generic;
using CCPDemo.RiskModels.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.RiskModels.Exporting
{
    public interface IRiskModelsExcelExporter
    {
        FileDto ExportToFile(List<GetRiskModelForViewDto> riskModels);
    }
}