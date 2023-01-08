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
                        L("Summary"),
                        L("ExistingControl"),
                        L("ERMRecommendation"),
                        L("ActionPlan"),
                        L("RiskOwnerComment"),
                        L("TargetDate"),
                        L("ActualClosureDate"),
                        L("AcceptanceDate"),
                        L("RiskAccepted"),
                        (L("RiskType")) + L("Name"),
                        (L("OrganizationUnit")) + L("DisplayName"),
                        (L("Status")) + L("Name"),
                        (L("RiskRating")) + L("Name"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, risks,
                        _ => _.Risk.Summary,
                        _ => _.Risk.ExistingControl,
                        _ => _.Risk.ERMRecommendation,
                        _ => _.Risk.ActionPlan,
                        _ => _.Risk.RiskOwnerComment,
                        _ => _timeZoneConverter.Convert(_.Risk.TargetDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Risk.ActualClosureDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Risk.AcceptanceDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Risk.RiskAccepted,
                        _ => _.RiskTypeName,
                        _ => _.OrganizationUnitDisplayName,
                        _ => _.StatusName,
                        _ => _.RiskRatingName,
                        _ => _.UserName
                        );

                    for (var i = 1; i <= risks.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[6], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(6); for (var i = 1; i <= risks.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[7], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(7); for (var i = 1; i <= risks.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[8], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(8);
                });
        }
    }
}