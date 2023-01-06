using System.Collections.Generic;
using CCPDemo.ThreatCatalogs.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.ThreatCatalogs.Exporting
{
    public interface IThreatCatalogExcelExporter
    {
        FileDto ExportToFile(List<GetThreatCatalogForViewDto> threatCatalog);
    }
}