using System.Collections.Generic;
using CCPDemo.RiskRatings.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.RiskRatings.Exporting
{
    public interface IRiskRatingsExcelExporter
    {
        FileDto ExportToFile(List<GetRiskRatingForViewDto> riskRatings);
    }
}