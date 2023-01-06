using System.Collections.Generic;
using CCPDemo.RiskCloseReason.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.RiskCloseReason.Exporting
{
    public interface ICloseReasonsExcelExporter
    {
        FileDto ExportToFile(List<GetCloseReasonForViewDto> closeReasons);
    }
}