using System.Collections.Generic;
using CCPDemo.Risks.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.Risks.Exporting
{
    public interface IRisksExcelExporter
    {
        FileDto ExportToFile(List<GetRiskForViewDto> risks);
    }
}