using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.KeyRiskIndicators.Dtos
{
    public class CreateOrEditKeyRiskIndicatorDto : EntityDto<int?>
    {

        [Required]
        public string ReferenceId { get; set; }

        public string BusinessLines { get; set; }

        public string Activity { get; set; }

        public string Process { get; set; }

        public string SubProcess { get; set; }

        public string PotentialRisk { get; set; }

        public string LikelihoodOfOccurrence_irr { get; set; }

        public string LikelihoodOfImpact_irr { get; set; }

        public string KeyControl { get; set; }

        public bool IsControlInUse { get; set; }

        public string ControlEffectiveness { get; set; }

        public string LikelihoodOfOccurrence_rrr { get; set; }

        public string LikelihoodOfImpact_rrr { get; set; }

        public string MitigationPlan { get; set; }

        public string Comment { get; set; }

        public string Status { get; set; }

        public string OwnerComment { get; set; }

    }
}