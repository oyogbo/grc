using System.Collections.Generic;
using CCPDemo.ReviewLevels.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.ReviewLevels.Exporting
{
    public interface IReviewLevelsExcelExporter
    {
        FileDto ExportToFile(List<GetReviewLevelForViewDto> reviewLevels);
    }
}