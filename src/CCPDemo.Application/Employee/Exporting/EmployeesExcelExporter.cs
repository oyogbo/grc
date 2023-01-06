using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.Employee.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.Employee.Exporting
{
    public class EmployeesExcelExporter : NpoiExcelExporterBase, IEmployeesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmployeesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmployeesForViewDto> employees)
        {
            return CreateExcelPackage(
                "Employees.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Employees"));

                    AddHeader(
                        sheet,
                        L("EmployeeID"),
                        L("LastName"),
                        L("FirstName"),
                        L("Title"),
                        L("TitleOfCourtesy"),
                        L("BirthDate"),
                        L("HireDate"),
                        L("Address"),
                        L("City"),
                        L("Region"),
                        L("PostalCode"),
                        L("Country"),
                        L("HomePhone"),
                        L("Extension"),
                        L("Photo"),
                        L("Notes"),
                        L("ReportsTo"),
                        L("PhotoPath")
                        );

                    AddObjects(
                        sheet, employees,
                        _ => _.Employees.EmployeeID,
                        _ => _.Employees.LastName,
                        _ => _.Employees.FirstName,
                        _ => _.Employees.Title,
                        _ => _.Employees.TitleOfCourtesy,
                        _ => _timeZoneConverter.Convert(_.Employees.BirthDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Employees.HireDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Employees.Address,
                        _ => _.Employees.City,
                        _ => _.Employees.Region,
                        _ => _.Employees.PostalCode,
                        _ => _.Employees.Country,
                        _ => _.Employees.HomePhone,
                        _ => _.Employees.Extension,
                        _ => _.Employees.Photo,
                        _ => _.Employees.Notes,
                        _ => _.Employees.ReportsTo,
                        _ => _.Employees.PhotoPath
                        );

                    for (var i = 1; i <= employees.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[6], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(6); for (var i = 1; i <= employees.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[7], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(7);
                });
        }
    }
}