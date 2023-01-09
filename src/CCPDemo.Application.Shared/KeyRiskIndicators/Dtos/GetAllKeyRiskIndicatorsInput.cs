using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.KeyRiskIndicators.Dtos
{
    public class GetAllKeyRiskIndicatorsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string ReferenceIdFilter { get; set; }

        public string BusinessLinesFilter { get; set; }

        public string ActivityFilter { get; set; }

        public string ProcessFilter { get; set; }

        public string SubProcessFilter { get; set; }

        public string PotentialRiskFilter { get; set; }

        public string LikelihoodOfOccurrence_irrFilter { get; set; }

        public string LikelihoodOfImpact_irrFilter { get; set; }

        public string KeyControlFilter { get; set; }

        public int? IsControlInUseFilter { get; set; }

        public string ControlEffectivenessFilter { get; set; }

        public string LikelihoodOfOccurrence_rrrFilter { get; set; }

        public string LikelihoodOfImpact_rrrFilter { get; set; }

        public string MitigationPlanFilter { get; set; }

        public string CommentFilter { get; set; }

        public string StatusFilter { get; set; }

        public string OwnerCommentFilter { get; set; }

    }
}