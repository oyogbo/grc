using System.Collections.Generic;
using CCPDemo.Projects.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.Projects.Exporting
{
    public interface IProjectsExcelExporter
    {
        FileDto ExportToFile(List<GetProjectForViewDto> projects);
    }
}