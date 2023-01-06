using System.Collections.Generic;
using CCPDemo.ThreatGroupings.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.ThreatGroupings.Exporting
{
    public interface IThreatGroupingExcelExporter
    {
        FileDto ExportToFile(List<GetThreatGroupingForViewDto> threatGrouping);
    }
}