using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.RiskLevels.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.RiskLevels.Exporting
{
    public class RiskLevelsExcelExporter : NpoiExcelExporterBase, IRiskLevelsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RiskLevelsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRiskLevelForViewDto> riskLevels)
        {
            return CreateExcelPackage(
                "RiskLevels.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("RiskLevels"));

                    AddHeader(
                        sheet,
                        L("Value"),
                        L("Name"),
                        L("Color"),
                        L("DisplayName")
                        );

                    AddObjects(
                        sheet, riskLevels,
                        _ => _.RiskLevel.Value,
                        _ => _.RiskLevel.Name,
                        _ => _.RiskLevel.Color,
                        _ => _.RiskLevel.DisplayName
                        );

                });
        }
    }
}