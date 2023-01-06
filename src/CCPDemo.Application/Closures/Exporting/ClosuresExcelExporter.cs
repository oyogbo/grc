using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.Closures.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.Closures.Exporting
{
    public class ClosuresExcelExporter : NpoiExcelExporterBase, IClosuresExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ClosuresExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetClosureForViewDto> closures)
        {
            return CreateExcelPackage(
                "Closures.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Closures"));

                    AddHeader(
                        sheet,
                        L("RiskId"),
                        L("UserId"),
                        L("ClosureDate"),
                        L("CloseReasonId"),
                        L("Note")
                        );

                    AddObjects(
                        sheet, closures,
                        _ => _.Closure.RiskId,
                        _ => _.Closure.UserId,
                        _ => _timeZoneConverter.Convert(_.Closure.ClosureDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Closure.CloseReasonId,
                        _ => _.Closure.Note
                        );

                    for (var i = 1; i <= closures.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(3);
                });
        }
    }
}