using Abp.Application.Services.Dto;
using CCPDemo.Risks;
using CCPDemo.Risks.Dtos;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CCPDemo.Web.Areas.App.Models.Risks
{
    public class RisksViewModel
    {
        public string FilterText { get; set; }

        public List<RiskRiskTypeLookupTableDto> RiskTypeList { get; set; }

        public List<RiskOrganizationUnitLookupTableDto> RiskOrganizationUnitList { get; set; }

        public List<RiskStatusLookupTableDto> RiskStatusList { get; set; }

        public List<RiskRiskRatingLookupTableDto> RiskRatingList { get; set; }

        public List<GetRiskForReports> Risks { get; set; }
        public List<GetRiskForViewDto> RisksList { get; set; }
        public List<RiskTypeByDepartment> RiskTypesByDepartment { get; set; }

    }
}