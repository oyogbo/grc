using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.RiskCloseReason.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.RiskCloseReason.Exporting
{
    public class CloseReasonsExcelExporter : NpoiExcelExporterBase, ICloseReasonsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CloseReasonsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCloseReasonForViewDto> closeReasons)
        {
            return CreateExcelPackage(
                "CloseReasons.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("CloseReasons"));

                    AddHeader(
                        sheet,
                        L("value"),
                        L("name")
                        );

                    AddObjects(
                        sheet, closeReasons,
                        _ => _.CloseReason.value,
                        _ => _.CloseReason.name
                        );

                });
        }
    }
}