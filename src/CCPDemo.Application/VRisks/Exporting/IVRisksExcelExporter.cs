using System.Collections.Generic;
using CCPDemo.VRisks.Dtos;
using CCPDemo.Dto;

namespace CCPDemo.VRisks.Exporting
{
    public interface IVRisksExcelExporter
    {
        FileDto ExportToFile(List<GetVRiskForViewDto> vRisks);
    }
}