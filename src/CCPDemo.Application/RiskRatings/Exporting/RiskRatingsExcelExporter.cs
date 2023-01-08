using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.RiskRatings.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.RiskRatings.Exporting
{
    public class RiskRatingsExcelExporter : NpoiExcelExporterBase, IRiskRatingsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RiskRatingsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRiskRatingForViewDto> riskRatings)
        {
            return CreateExcelPackage(
                "RiskRatings.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("RiskRatings"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Color")
                        );

                    AddObjects(
                        sheet, riskRatings,
                        _ => _.RiskRating.Name,
                        _ => _.RiskRating.Color
                        );

                });
        }
    }
}