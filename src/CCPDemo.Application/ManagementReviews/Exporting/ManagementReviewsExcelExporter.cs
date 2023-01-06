using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.ManagementReviews.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.ManagementReviews.Exporting
{
    public class ManagementReviewsExcelExporter : NpoiExcelExporterBase, IManagementReviewsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ManagementReviewsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetManagementReviewForViewDto> managementReviews)
        {
            return CreateExcelPackage(
                "ManagementReviews.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ManagementReviews"));

                    AddHeader(
                        sheet,
                        L("risk_id"),
                        L("submission_date"),
                        L("review"),
                        L("reviewer"),
                        L("next_step"),
                        L("comments"),
                        L("next_review")
                        );

                    AddObjects(
                        sheet, managementReviews,
                        _ => _.ManagementReview.risk_id,
                        _ => _timeZoneConverter.Convert(_.ManagementReview.submission_date, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ManagementReview.review,
                        _ => _.ManagementReview.reviewer,
                        _ => _.ManagementReview.next_step,
                        _ => _.ManagementReview.comments,
                        _ => _timeZoneConverter.Convert(_.ManagementReview.next_review, _abpSession.TenantId, _abpSession.GetUserId())
                        );

                    for (var i = 1; i <= managementReviews.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[2], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(2); for (var i = 1; i <= managementReviews.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[7], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(7);
                });
        }
    }
}