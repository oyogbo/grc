using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.RiskModels.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.RiskModels.Exporting
{
    public class RiskModelsExcelExporter : NpoiExcelExporterBase, IRiskModelsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RiskModelsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRiskModelForViewDto> riskModels)
        {
            return CreateExcelPackage(
                "RiskModels.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("RiskModels"));

                    AddHeader(
                        sheet,
                        L("Value"),
                        L("Name")
                        );

                    AddObjects(
                        sheet, riskModels,
                        _ => _.RiskModel.Value,
                        _ => _.RiskModel.Name
                        );

                });
        }
    }
}