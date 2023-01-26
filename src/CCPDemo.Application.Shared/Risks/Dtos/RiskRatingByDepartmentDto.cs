using System;
using System.Collections.Generic;
using System.Text;

namespace CCPDemo.Risks.Dtos
{
    public class RiskRatingByDepartmentDto
    {
        public string OrganizationUnit { get; set; }
        public string RiskRating { get; set; }
        public string RatingCount { get; set; }
    }
}
