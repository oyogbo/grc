using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.RiskStatus.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.RiskStatus.Exporting
{
    public class StatusExcelExporter : NpoiExcelExporterBase, IStatusExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StatusExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStatusForViewDto> status)
        {
            return CreateExcelPackage(
                "Status.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Status"));

                    AddHeader(
                        sheet,
                        L("Name")
                        );

                    AddObjects(
                        sheet, status,
                        _ => _.Status.Name
                        );

                });
        }
    }
}