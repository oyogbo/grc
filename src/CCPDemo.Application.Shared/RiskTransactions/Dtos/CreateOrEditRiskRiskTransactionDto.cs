using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.RiskTransactions.Dtos
{
    public class CreateOrEditRiskRiskTransactionDto : EntityDto<int?>
    {

        public string TransactionType { get; set; }

        public string Date { get; set; }

        public string CurrentValue { get; set; }

        public string NewValue { get; set; }

        public int RiskId { get; set; }

        public long riskTransferrerUserId { get; set; }

        // Raise a new Risk
        public string Summary { get; set; }

        public string ExistingControl { get; set; }

        public string ERMRecommendation { get; set; }

        public string ActionPlan { get; set; }

        public string RiskOwnerComment { get; set; }

        public DateTime TargetDate { get; set; }

        public DateTime ActualClosureDate { get; set; }

        public DateTime AcceptanceDate { get; set; }

        public bool RiskAccepted { get; set; }

        public int RiskTypeId { get; set; }

        public long? OrganizationUnitId { get; set; }

        public int? StatusId { get; set; }

        public int? RiskRatingId { get; set; }

        public long? UserId { get; set; }

    }
}