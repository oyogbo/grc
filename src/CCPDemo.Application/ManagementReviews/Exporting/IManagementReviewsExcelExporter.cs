using System.Collections.Generic;
using CCPDemo.ManagementReviews.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.ManagementReviews.Exporting
{
    public interface IManagementReviewsExcelExporter
    {
        FileDto ExportToFile(List<GetManagementReviewForViewDto> managementReviews);
    }
}