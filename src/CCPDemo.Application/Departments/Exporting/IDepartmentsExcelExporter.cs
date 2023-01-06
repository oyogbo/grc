using System.Collections.Generic;
using CCPDemo.Departments.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.Departments.Exporting
{
    public interface IDepartmentsExcelExporter
    {
        FileDto ExportToFile(List<GetDepartmentForViewDto> departments);
    }
}