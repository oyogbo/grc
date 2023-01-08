using CCPDemo.Risks.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;

using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Models.Risks
{
    public class CreateOrEditRiskModalViewModel
    {
        public CreateOrEditRiskDto Risk { get; set; }

        public string RiskTypeName { get; set; }

        public string OrganizationUnitDisplayName { get; set; }

        public string StatusName { get; set; }

        public string RiskRatingName { get; set; }

        public string UserName { get; set; }

        public List<RiskRiskTypeLookupTableDto> RiskRiskTypeList { get; set; }

        public List<RiskOrganizationUnitLookupTableDto> RiskOrganizationUnitList { get; set; }

        public List<RiskStatusLookupTableDto> RiskStatusList { get; set; }

        public List<RiskRiskRatingLookupTableDto> RiskRiskRatingList { get; set; }

        public List<RiskUserLookupTableDto> RiskUserList { get; set; }

        public bool IsEditMode => Risk.Id.HasValue;
    }
}