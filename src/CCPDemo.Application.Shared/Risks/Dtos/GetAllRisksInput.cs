using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.Risks.Dtos
{
    public class GetAllRisksInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string SummaryFilter { get; set; }

        public string ExistingControlFilter { get; set; }

        public string ERMRecommendationFilter { get; set; }

        public string ActionPlanFilter { get; set; }

        public string RiskOwnerCommentFilter { get; set; }

        public DateTime? MaxTargetDateFilter { get; set; }
        public DateTime? MinTargetDateFilter { get; set; }

        public DateTime? MaxActualClosureDateFilter { get; set; }
        public DateTime? MinActualClosureDateFilter { get; set; }

        public DateTime? MaxAcceptanceDateFilter { get; set; }
        public DateTime? MinAcceptanceDateFilter { get; set; }

        public int? RiskAcceptedFilter { get; set; }

        public string RiskTypeNameFilter { get; set; }

        public string OrganizationUnitDisplayNameFilter { get; set; }

        public string StatusNameFilter { get; set; }

        public string RiskRatingNameFilter { get; set; }

        public string UserNameFilter { get; set; }

    }
}