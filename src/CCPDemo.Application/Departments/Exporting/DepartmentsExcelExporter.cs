using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.Departments.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.Departments.Exporting
{
    public class DepartmentsExcelExporter : NpoiExcelExporterBase, IDepartmentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DepartmentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDepartmentForViewDto> departments)
        {
            return CreateExcelPackage(
                "Departments.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Departments"));

                    AddHeader(
                        sheet,
                        L("Name")
                        );

                    AddObjects(
                        sheet, departments,
                        _ => _.Department.Name
                        );

                });
        }
    }
}