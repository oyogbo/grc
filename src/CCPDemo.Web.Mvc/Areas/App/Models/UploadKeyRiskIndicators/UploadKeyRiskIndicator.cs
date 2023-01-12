using Abp.Organizations;
using System.Collections.Generic;

namespace CCPDemo.Web.Areas.App.Models.UploadKeyRiskIndicators
{
    public class UploadKeyRiskIndicator
    {
        public List<OrganizationUnit> OrganizationUnits { get; set; } = new List<OrganizationUnit>();
    }
}
