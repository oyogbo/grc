using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.Projects.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.Projects.Exporting
{
    public class ProjectsExcelExporter : NpoiExcelExporterBase, IProjectsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProjectsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProjectForViewDto> projects)
        {
            return CreateExcelPackage(
                "Projects.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Projects"));

                    AddHeader(
                        sheet,
                        L("Value"),
                        L("Name"),
                        L("DueDate"),
                        L("ConsultantId"),
                        L("BusinessOwnerId"),
                        L("DataClassificationId"),
                        L("Order"),
                        L("status")
                        );

                    AddObjects(
                        sheet, projects,
                        _ => _.Project.Value,
                        _ => _.Project.Name,
                        _ => _timeZoneConverter.Convert(_.Project.DueDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Project.ConsultantId,
                        _ => _.Project.BusinessOwnerId,
                        _ => _.Project.DataClassificationId,
                        _ => _.Project.Order,
                        _ => _.Project.status
                        );

                    for (var i = 1; i <= projects.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(3);
                });
        }
    }
}