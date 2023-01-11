using CCPDemo.Risks.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;

using Abp.Extensions;
using CCPDemo.Authorization.Roles;

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
        public bool isAdmin { get; set; }
        public bool isERM { get; set; }

        public string userText { get; set; }
        public string ERMText { get; set; }
        public List<string> RoleNames { get; set; }
        public string RoleName { get; set; }

        public Dictionary<int, string> RoleList { get; set; }

        public Dictionary<int, int> UserRolesInfo { get; set; }
        public IList<string> UserRoles { get; set;}
        public Dictionary<string, int> UsersEmailsIdsDict { get; set; }
    }
}