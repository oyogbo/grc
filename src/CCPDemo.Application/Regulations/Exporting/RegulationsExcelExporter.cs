using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.Regulations.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.Regulations.Exporting
{
    public class RegulationsExcelExporter : NpoiExcelExporterBase, IRegulationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RegulationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRegulationForViewDto> regulations)
        {
            return CreateExcelPackage(
                "Regulations.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Regulations"));

                    AddHeader(
                        sheet,
                        L("value"),
                        L("name")
                        );

                    AddObjects(
                        sheet, regulations,
                        _ => _.Regulation.value,
                        _ => _.Regulation.name
                        );

                });
        }
    }
}