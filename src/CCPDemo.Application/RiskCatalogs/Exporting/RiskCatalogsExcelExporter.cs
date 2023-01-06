using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.RiskCatalogs.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.RiskCatalogs.Exporting
{
    public class RiskCatalogsExcelExporter : NpoiExcelExporterBase, IRiskCatalogsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RiskCatalogsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRiskCatalogForViewDto> riskCatalogs)
        {
            return CreateExcelPackage(
                "RiskCatalogs.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("RiskCatalogs"));

                    AddHeader(
                        sheet,
                        L("Number"),
                        L("Grouping"),
                        L("Name"),
                        L("Description"),
                        L("Function"),
                        L("Order")
                        );

                    AddObjects(
                        sheet, riskCatalogs,
                        _ => _.RiskCatalog.Number,
                        _ => _.RiskCatalog.Grouping,
                        _ => _.RiskCatalog.Name,
                        _ => _.RiskCatalog.Description,
                        _ => _.RiskCatalog.Function,
                        _ => _.RiskCatalog.Order
                        );

                });
        }
    }
}