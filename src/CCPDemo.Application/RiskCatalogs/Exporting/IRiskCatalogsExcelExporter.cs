using System.Collections.Generic;
using CCPDemo.RiskCatalogs.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.RiskCatalogs.Exporting
{
    public interface IRiskCatalogsExcelExporter
    {
        FileDto ExportToFile(List<GetRiskCatalogForViewDto> riskCatalogs);
    }
}