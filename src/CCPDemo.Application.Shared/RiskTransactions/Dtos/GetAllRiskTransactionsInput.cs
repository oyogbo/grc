using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.RiskTransactions.Dtos
{
    public class GetAllRiskTransactionsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string TransactionTypeFilter { get; set; }

        public string DateFilter { get; set; }

        public string CurrentValueFilter { get; set; }

        public string NewValueFilter { get; set; }

        public int? MaxRiskIdFilter { get; set; }
        public int? MinRiskIdFilter { get; set; }

        public long? MaxUserIdFilter { get; set; }
        public long? MinUserIdFilter { get; set; }

    }
}