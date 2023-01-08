using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCPDemo.Risks.Dtos
{
    public class ModifiedRiskTransactionsDto : EntityDto
    {
        public int riskId { get; set; }
        public int currentOwner { get; set;}
        public int riskTransferrerUserId { get; set;}
        public string transactionType { get; set; }
        public int riskTypeId { get; set; }
        public int organizationUnitId { get; set;}
        public int statusId { get; set;}
        public int riskRatingId { get; set; }
        public int userId { get;}
        public string summary { get; set;}
        public string existingControl { get; set;}
        public string ermRecommendation { get; set;}
        public string actionPlan { get; set; }
        public string riskOwnerComment { get; set; }
        public DateTime targetDate { get; set; }
        public DateTime actualClosureDate { get; set; }
        public DateTime acceptanceDate { get; set; }
        public bool riskAccepted { get; set; }
    }
}
