using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CCPDemo.DataExporting.Excel.NPOI;
using CCPDemo.RiskTransactions.Dtos;
using CCPDemo.Dto;
using CCPDemo.Storage;

namespace CCPDemo.RiskTransactions.Exporting
{
    public class RiskTransactionsExcelExporter : NpoiExcelExporterBase, IRiskTransactionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RiskTransactionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRiskTransactionForViewDto> riskTransactions)
        {
            return CreateExcelPackage(
                "RiskTransactions.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("RiskTransactions"));

                    AddHeader(
                        sheet,
                        L("TransactionType"),
                        L("Date"),
                        L("CurrentValue"),
                        L("NewValue"),
                        L("RiskId"),
                        L("UserId")
                        );

                    AddObjects(
                        sheet, riskTransactions,
                        _ => _.RiskTransaction.TransactionType,
                        _ => _.RiskTransaction.Date,
                        _ => _.RiskTransaction.CurrentValue,
                        _ => _.RiskTransaction.NewValue,
                        _ => _.RiskTransaction.RiskId,
                        _ => _.RiskTransaction.UserId
                        );

                });
        }
    }
}