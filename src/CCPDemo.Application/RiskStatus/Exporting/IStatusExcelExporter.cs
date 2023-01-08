using System.Collections.Generic;
using CCPDemo.RiskStatus.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.RiskStatus.Exporting
{
    public interface IStatusExcelExporter
    {
        FileDto ExportToFile(List<GetStatusForViewDto> status);
    }
}