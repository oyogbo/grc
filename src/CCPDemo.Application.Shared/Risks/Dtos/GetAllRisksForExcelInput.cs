using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.Risks.Dtos
{
    public class GetAllRisksForExcelInput
    {
        public string Filter { get; set; }

        public string RiskTypeFilter { get; set; }

        public string RiskSummaryFilter { get; set; }

        public string DepartmentFilter { get; set; }

        public string RiskOwnerFilter { get; set; }

        public string ExistingControlFilter { get; set; }

        public string ERMCommentFilter { get; set; }

        public string ActionPlanFilter { get; set; }

        public string RiskOwnerCommentFilter { get; set; }

        public string StatusFilter { get; set; }

        public string RatingFilter { get; set; }

        public string TargetDateFilter { get; set; }

        public string ActualClosureDateFilter { get; set; }

        public int? RiskAcceptedFilter { get; set; }

    }
}