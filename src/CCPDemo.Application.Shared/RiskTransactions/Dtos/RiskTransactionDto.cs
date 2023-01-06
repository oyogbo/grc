using System;
using Abp.Application.Services.Dto;

namespace CCPDemo.RiskTransactions.Dtos
{
    public class RiskTransactionDto : EntityDto
    {
        public string TransactionType { get; set; }

        public string Date { get; set; }

        public string CurrentValue { get; set; }

        public string NewValue { get; set; }

        public int RiskId { get; set; }

        public long UserId { get; set; }

    }
}