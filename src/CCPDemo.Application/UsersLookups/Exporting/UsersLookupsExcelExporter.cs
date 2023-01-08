using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.UsersLookups.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.UsersLookups.Exporting
{
    public class UsersLookupsExcelExporter : NpoiExcelExporterBase, IUsersLookupsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public UsersLookupsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetUsersLookupForViewDto> usersLookups)
        {
            return CreateExcelPackage(
                "UsersLookups.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("UsersLookups"));

                    AddHeader(
                        sheet,
                        L("User"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, usersLookups,
                        _ => _.UsersLookup.User,
                        _ => _.UserName
                        );

                });
        }
    }
}