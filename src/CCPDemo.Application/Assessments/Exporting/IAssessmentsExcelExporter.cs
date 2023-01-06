using System.Collections.Generic;
using CCPDemo.Assessments.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.Assessments.Exporting
{
    public interface IAssessmentsExcelExporter
    {
        FileDto ExportToFile(List<GetAssessmentForViewDto> assessments);
    }
}