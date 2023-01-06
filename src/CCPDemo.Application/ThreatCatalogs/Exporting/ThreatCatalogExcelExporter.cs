using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.ThreatCatalogs.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.ThreatCatalogs.Exporting
{
    public class ThreatCatalogExcelExporter : NpoiExcelExporterBase, IThreatCatalogExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ThreatCatalogExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetThreatCatalogForViewDto> threatCatalog)
        {
            return CreateExcelPackage(
                "ThreatCatalog.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ThreatCatalog"));

                    AddHeader(
                        sheet,
                        L("Number"),
                        L("Grouping"),
                        L("Name"),
                        L("Description"),
                        L("Order")
                        );

                    AddObjects(
                        sheet, threatCatalog,
                        _ => _.ThreatCatalog.Number,
                        _ => _.ThreatCatalog.Grouping,
                        _ => _.ThreatCatalog.Name,
                        _ => _.ThreatCatalog.Description,
                        _ => _.ThreatCatalog.Order
                        );

                });
        }
    }
}