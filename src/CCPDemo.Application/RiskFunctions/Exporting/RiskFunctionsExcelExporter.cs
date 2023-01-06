using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.RiskFunctions.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.RiskFunctions.Exporting
{
    public class RiskFunctionsExcelExporter : NpoiExcelExporterBase, IRiskFunctionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RiskFunctionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRiskFunctionForViewDto> riskFunctions)
        {
            return CreateExcelPackage(
                "RiskFunctions.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("RiskFunctions"));

                    AddHeader(
                        sheet,
                        L("Value"),
                        L("Name")
                        );

                    AddObjects(
                        sheet, riskFunctions,
                        _ => _.RiskFunction.Value,
                        _ => _.RiskFunction.Name
                        );

                });
        }
    }
}