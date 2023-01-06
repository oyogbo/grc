using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.RiskTransactions.Dtos
{
    public class GetRiskTransactionForEditOutput
    {
        public CreateOrEditRiskTransactionDto RiskTransaction { get; set; }

    }
}