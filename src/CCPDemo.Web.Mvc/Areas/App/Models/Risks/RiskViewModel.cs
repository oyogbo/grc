﻿using CCPDemo.Authorization.Users;
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

    }
}