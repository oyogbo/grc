using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCPDemo.KeyRiskIndicators.Dtos
{
    public class KeyRiskIndicatorUploadDto : EntityDto
    {
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
    }
}
