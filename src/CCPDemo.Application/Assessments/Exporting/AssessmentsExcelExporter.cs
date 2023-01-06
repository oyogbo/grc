using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.Assessments.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.Assessments.Exporting
{
    public class AssessmentsExcelExporter : NpoiExcelExporterBase, IAssessmentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AssessmentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAssessmentForViewDto> assessments)
        {
            return CreateExcelPackage(
                "Assessments.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Assessments"));

                    AddHeader(
                        sheet,
                        L("Name")
                        );

                    AddObjects(
                        sheet, assessments,
                        _ => _.Assessment.Name
                        );

                });
        }
    }
}