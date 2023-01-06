using System.Collections.Generic;
using CCPDemo.Closures.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.Closures.Exporting
{
    public interface IClosuresExcelExporter
    {
        FileDto ExportToFile(List<GetClosureForViewDto> closures);
    }
}