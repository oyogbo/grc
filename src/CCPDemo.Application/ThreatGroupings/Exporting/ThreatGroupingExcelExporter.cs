using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.ThreatGroupings.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.ThreatGroupings.Exporting
{
    public class ThreatGroupingExcelExporter : NpoiExcelExporterBase, IThreatGroupingExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ThreatGroupingExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetThreatGroupingForViewDto> threatGrouping)
        {
            return CreateExcelPackage(
                "ThreatGrouping.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ThreatGrouping"));

                    AddHeader(
                        sheet,
                        L("Value"),
                        L("Name"),
                        L("Default"),
                        L("Order")
                        );

                    AddObjects(
                        sheet, threatGrouping,
                        _ => _.ThreatGrouping.Value,
                        _ => _.ThreatGrouping.Name,
                        _ => _.ThreatGrouping.Default,
                        _ => _.ThreatGrouping.Order
                        );

                });
        }
    }
}