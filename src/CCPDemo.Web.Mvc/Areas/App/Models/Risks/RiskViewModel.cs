using CCPDemo.Authorization.Users;
using CCPDemo.Risks;
using CCPDemo.Risks.Dtos;
using System;
using System.Collections.Generic;

namespace CCPDemo.Web.Areas.App.Models.Risks
{
    public class RiskViewModel : GetRiskForViewDto
    {
        public string FilterText { get; set; }

        public int? RiskCount { get; set; }
        public int? KeyRiskIndicatorCount { get; set; }

        public User User { get; set; }

        public List<Risk> Risks { get; set; }
        public int? Low { get; set; }
        public int? Medium { get; set; }
        public int? High { get; set; }
        public int? VeryHigh { get; set; }
        public int? Critical { get; set; }

        public Array DepartmentsCounts { get; set; }
        public List<RiskTableForDisplayDto> dbList { get; set; }

        public bool isERM { get; set; }
        public List<string> OrganizationUnitLabels { get; set; }
        public List<string> RiskTypeLabels { get; set; }
        public List<string[]> RiskTypeCountCountData { get; set; }
        public List<string> RiskTypeOrgUnitsUnits { get; set; }
        public List<string> RiskRatingOrgUnitsUnits { get; set; }
        public List<string> RiskTypes { get; set; }
        public List<string> RiskRating { get; set; }
        public string RiskCountByDepartment { get; set; }
        public List<RiskRatingByDepartmentDto> RatingCountByDepartment { get; set; }


	}
}