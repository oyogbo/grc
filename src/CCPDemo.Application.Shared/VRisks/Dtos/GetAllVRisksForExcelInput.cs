using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.VRisks.Dtos
{
    public class GetAllVRisksForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string DepartmentFilter { get; set; }

        public string RiskOwnerFilter { get; set; }

        public string ResolutionTimeLineFilter { get; set; }

        public string ERMCommentFilter { get; set; }

        public string RiskOwnerCommentFilter { get; set; }

        public string StatusFilter { get; set; }

        public string ActualClosureDateFilter { get; set; }

        public string MitigationDateFilter { get; set; }

        public int? AcceptRiskFilter { get; set; }

        public string AcceptanceDateFilter { get; set; }

    }
}