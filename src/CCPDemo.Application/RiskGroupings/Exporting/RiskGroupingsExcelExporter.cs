using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.RiskGroupings.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.RiskGroupings.Exporting
{
    public class RiskGroupingsExcelExporter : NpoiExcelExporterBase, IRiskGroupingsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RiskGroupingsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRiskGroupingForViewDto> riskGroupings)
        {
            return CreateExcelPackage(
                "RiskGroupings.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("RiskGroupings"));

                    AddHeader(
                        sheet,
                        L("Value"),
                        L("Name"),
                        L("Default"),
                        L("Order")
                        );

                    AddObjects(
                        sheet, riskGroupings,
                        _ => _.RiskGrouping.Value,
                        _ => _.RiskGrouping.Name,
                        _ => _.RiskGrouping.Default,
                        _ => _.RiskGrouping.Order
                        );

                });
        }
    }
}