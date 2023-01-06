using CCPDemo.RiskGroupings.Dtos;

using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Models.RiskGroupings
{
    public class CreateOrEditRiskGroupingModalViewModel
    {
        public CreateOrEditRiskGroupingDto RiskGrouping { get; set; }

        public bool IsEditMode => RiskGrouping.Id.HasValue;
    }
}