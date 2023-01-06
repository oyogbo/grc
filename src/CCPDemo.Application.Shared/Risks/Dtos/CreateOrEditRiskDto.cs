using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.Risks.Dtos
{
    public class CreateOrEditRiskDto : EntityDto<int?>
    {

        public string RiskType { get; set; }

        public string RiskSummary { get; set; }

        public string Department { get; set; }
        public long RiskOwnerId { get; set; }
        public string RiskOwner { get; set; }

        public string ExistingControl { get; set; }

        public string ERMComment { get; set; }

        public string ActionPlan { get; set; }

        public string RiskOwnerComment { get; set; }

        public string Status { get; set; }

        public string Rating { get; set; }

        public string TargetDate { get; set; }

        public string ActualClosureDate { get; set; }

        public string AcceptanceDate { get; set; }

        public bool RiskAccepted { get; set; }

    }
}