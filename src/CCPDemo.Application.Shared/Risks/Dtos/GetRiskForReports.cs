using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCPDemo.Risks.Dtos
{
    public class GetRiskForReports : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public string RiskTypeNameFilter { get; set; }

        public string OrganizationUnitDisplayNameFilter { get; set; }

        public string RiskRatingNameFilter { get; set; }

    }
}
