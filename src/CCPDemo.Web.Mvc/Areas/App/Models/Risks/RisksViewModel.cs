using CCPDemo.Risks.Dtos;
using System.Collections.Generic;

namespace CCPDemo.Web.Areas.App.Models.Risks
{
    public class RisksViewModel
    {
        public string FilterText { get; set; }

        public List<RiskRiskTypeLookupTableDto> RiskTypeList { get; set; }

        public List<RiskOrganizationUnitLookupTableDto> RiskOrganizationUnitList { get; set; }

        public List<RiskStatusLookupTableDto> RiskStatusList { get; set; }

        public List<RiskRiskRatingLookupTableDto> RiskRiskRatingList { get; set; }

    }
}