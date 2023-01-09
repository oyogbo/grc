using System.Collections.Generic;
using CCPDemo.KeyRiskIndicators.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.KeyRiskIndicators.Exporting
{
    public interface IKeyRiskIndicatorsExcelExporter
    {
        FileDto ExportToFile(List<GetKeyRiskIndicatorForViewDto> keyRiskIndicators);
    }
}