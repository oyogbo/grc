using CCPDemo.RiskLevels.Dtos;

using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Models.RiskLevels
{
    public class CreateOrEditRiskLevelModalViewModel
    {
        public CreateOrEditRiskLevelDto RiskLevel { get; set; }

        public bool IsEditMode => RiskLevel.Id.HasValue;
    }
}