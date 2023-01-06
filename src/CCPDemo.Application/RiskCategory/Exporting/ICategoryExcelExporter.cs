using System.Collections.Generic;
using CCPDemo.RiskCategory.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.RiskCategory.Exporting
{
    public interface ICategoryExcelExporter
    {
        FileDto ExportToFile(List<GetCategoryForViewDto> category);
    }
}