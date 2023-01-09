using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.KeyRiskIndicators.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.KeyRiskIndicators.Exporting
{
    public class KeyRiskIndicatorsExcelExporter : NpoiExcelExporterBase, IKeyRiskIndicatorsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public KeyRiskIndicatorsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetKeyRiskIndicatorForViewDto> keyRiskIndicators)
        {
            return CreateExcelPackage(
                "KeyRiskIndicators.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("KeyRiskIndicators"));

                    AddHeader(
                        sheet,
                        L("BusinessLines"),
                        L("Activity"),
                        L("Process"),
                        L("SubProcess"),
                        L("PotentialRisk"),
                        L("LikelihoodOfOccurrence_irr"),
                        L("LikelihoodOfImpact_irr"),
                        L("KeyControl"),
                        L("IsControlInUse"),
                        L("ControlEffectiveness"),
                        L("LikelihoodOfOccurrence_rrr"),
                        L("LikelihoodOfImpact_rrr"),
                        L("MitigationPlan"),
                        L("Comment"),
                        L("Status"),
                        L("OwnerComment")
                        );

                    AddObjects(
                        sheet, keyRiskIndicators,
                        _ => _.KeyRiskIndicator.BusinessLines,
                        _ => _.KeyRiskIndicator.Activity,
                        _ => _.KeyRiskIndicator.Process,
                        _ => _.KeyRiskIndicator.SubProcess,
                        _ => _.KeyRiskIndicator.PotentialRisk,
                        _ => _.KeyRiskIndicator.LikelihoodOfOccurrence_irr,
                        _ => _.KeyRiskIndicator.LikelihoodOfImpact_irr,
                        _ => _.KeyRiskIndicator.KeyControl,
                        _ => _.KeyRiskIndicator.IsControlInUse,
                        _ => _.KeyRiskIndicator.ControlEffectiveness,
                        _ => _.KeyRiskIndicator.LikelihoodOfOccurrence_rrr,
                        _ => _.KeyRiskIndicator.LikelihoodOfImpact_rrr,
                        _ => _.KeyRiskIndicator.MitigationPlan,
                        _ => _.KeyRiskIndicator.Comment,
                        _ => _.KeyRiskIndicator.Status,
                        _ => _.KeyRiskIndicator.OwnerComment
                        );

                });
        }
    }
}