using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.ReviewLevels.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.ReviewLevels.Exporting
{
    public class ReviewLevelsExcelExporter : NpoiExcelExporterBase, IReviewLevelsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ReviewLevelsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetReviewLevelForViewDto> reviewLevels)
        {
            return CreateExcelPackage(
                "ReviewLevels.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ReviewLevels"));

                    AddHeader(
                        sheet,
                        L("Value"),
                        L("Name")
                        );

                    AddObjects(
                        sheet, reviewLevels,
                        _ => _.ReviewLevel.Value,
                        _ => _.ReviewLevel.Name
                        );

                });
        }
    }
}