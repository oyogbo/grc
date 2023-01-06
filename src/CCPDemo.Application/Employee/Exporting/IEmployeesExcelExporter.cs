using System.Collections.Generic;
using CCPDemo.Employee.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.Employee.Exporting
{
    public interface IEmployeesExcelExporter
    {
        FileDto ExportToFile(List<GetEmployeesForViewDto> employees);
    }
}