using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.RiskTypes.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.RiskTypes.Exporting
{
    public class RiskTypesExcelExporter : NpoiExcelExporterBase, IRiskTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RiskTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRiskTypeForViewDto> riskTypes)
        {
            return CreateExcelPackage(
                "RiskTypes.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("RiskTypes"));

                    AddHeader(
                        sheet,
                        L("Name")
                        );

                    AddObjects(
                        sheet, riskTypes,
                        _ => _.RiskType.Name
                        );

                });
        }
    }
}