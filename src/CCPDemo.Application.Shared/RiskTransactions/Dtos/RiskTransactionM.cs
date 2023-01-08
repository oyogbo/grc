using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCPDemo.RiskTransactions.Dtos
{
    public class RiskTransactionM : EntityDto
    {
        public string TransactionType { get; set; }

        public string Date { get; set; }

        public int CurrentValue { get; set; }

        public int NewValue { get; set; }

        public int RiskId { get; set; }

        public long UserId { get; set; }

    }
}
