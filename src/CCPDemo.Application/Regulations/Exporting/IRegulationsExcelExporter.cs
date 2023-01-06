using System.Collections.Generic;
using CCPDemo.Regulations.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.Regulations.Exporting
{
    public interface IRegulationsExcelExporter
    {
        FileDto ExportToFile(List<GetRegulationForViewDto> regulations);
    }
}