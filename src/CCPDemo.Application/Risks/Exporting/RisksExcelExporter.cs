using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.Risks.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.Risks.Exporting
{
    public class RisksExcelExporter : NpoiExcelExporterBase, IRisksExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RisksExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRiskForViewDto> risks)
        {
            return CreateExcelPackage(
                "Risks.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Risks"));

                    AddHeader(
                        sheet,
                        L("RiskType"),
                        L("RiskSummary"),
                        L("Department"),
                        L("RiskOwner"),
                        L("ExistingControl"),
                        L("ERMComment"),
                        L("ActionPlan"),
                        L("RiskOwnerComment"),
                        L("Status"),
                        L("Rating"),
                        L("TargetDate"),
                        L("ActualClosureDate"),
                        L("RiskAccepted")
                        );

                    AddObjects(
                        sheet, risks,
                        _ => _.Risk.RiskType,
                        _ => _.Risk.RiskSummary,
                        _ => _.Risk.Department,
                        _ => _.Risk.RiskOwner,
                        _ => _.Risk.ExistingControl,
                        _ => _.Risk.ERMComment,
                        _ => _.Risk.ActionPlan,
                        _ => _.Risk.RiskOwnerComment,
                        _ => _.Risk.Status,
                        _ => _.Risk.Rating,
                        _ => _.Risk.TargetDate,
                        _ => _.Risk.ActualClosureDate,
                        _ => _.Risk.RiskAccepted
                        );

                });
        }
    }
}