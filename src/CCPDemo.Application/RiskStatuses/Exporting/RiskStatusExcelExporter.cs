using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.RiskStatuses.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.RiskStatuses.Exporting
{
    public class RiskStatusExcelExporter : NpoiExcelExporterBase, IRiskStatusExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RiskStatusExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRiskStatusForViewDto> riskStatus)
        {
            return CreateExcelPackage(
                "RiskStatus.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("RiskStatus"));

                    AddHeader(
                        sheet,
                        L("Name")
                        );

                    AddObjects(
                        sheet, riskStatus,
                        _ => _.RiskStatus.Name
                        );

                });
        }
    }
}