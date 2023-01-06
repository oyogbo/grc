using CCPDemo.Authorization.Users;
using System.Collections;
using CCPDemo.VRisks;
using System.Collections.Generic;
using System;

namespace CCPDemo.Web.Areas.App.Models.VRisks
{
    public class VRisksViewModel
    {
        public string FilterText { get; set; }

        public int? RiskCount { get; set; }
        public int? KeyRiskIndicatorCount { get; set; }

        public User User { get; set; }

        public List<VRisk> Risks { get; set; }
        public int? Low { get; set; }
        public int? Medium { get; set; }
        public int? High { get; set; }
        public int? Critical { get; set; }

        public Array DepartmentsCounts { get; set; }

    }
}