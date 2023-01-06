using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.VRisks.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.VRisks.Exporting
{
    public class VRisksExcelExporter : NpoiExcelExporterBase, IVRisksExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public VRisksExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetVRiskForViewDto> vRisks)
        {
            return CreateExcelPackage(
                "VRisks.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("VRisks"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Department"),
                        L("RiskOwner"),
                        L("ResolutionTimeLine"),
                        L("ERMComment"),
                        L("RiskOwnerComment"),
                        L("Status"),
                        L("ActualClosureDate"),
                        L("MitigationDate"),
                        L("AcceptRisk"),
                        L("AcceptanceDate")
                        );

                    AddObjects(
                        sheet, vRisks,
                        _ => _.VRisk.Name,
                        _ => _.VRisk.Description,
                        _ => _.VRisk.Department,
                        _ => _.VRisk.RiskOwner,
                        _ => _.VRisk.ResolutionTimeLine,
                        _ => _.VRisk.ERMComment,
                        _ => _.VRisk.RiskOwnerComment,
                        _ => _.VRisk.Status,
                        _ => _.VRisk.ActualClosureDate,
                        _ => _.VRisk.MitigationDate,
                        _ => _.VRisk.AcceptRisk,
                        _ => _.VRisk.AcceptanceDate
                        );

                });
        }
    }
}