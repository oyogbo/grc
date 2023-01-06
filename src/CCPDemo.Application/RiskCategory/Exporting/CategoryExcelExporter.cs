using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.RiskCategory.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.RiskCategory.Exporting
{
    public class CategoryExcelExporter : NpoiExcelExporterBase, ICategoryExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CategoryExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCategoryForViewDto> category)
        {
            return CreateExcelPackage(
                "Category.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Category"));

                    AddHeader(
                        sheet,
                        L("Value"),
                        L("Name")
                        );

                    AddObjects(
                        sheet, category,
                        _ => _.Category.Value,
                        _ => _.Category.Name
                        );

                });
        }
    }
}