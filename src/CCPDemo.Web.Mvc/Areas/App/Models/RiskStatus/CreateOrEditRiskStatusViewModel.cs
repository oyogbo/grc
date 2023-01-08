using CCPDemo.RiskStatuses.Dtos;

using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Models.RiskStatus
{
    public class CreateOrEditRiskStatusModalViewModel
    {
        public CreateOrEditRiskStatusDto RiskStatus { get; set; }

        public bool IsEditMode => RiskStatus.Id.HasValue;
    }
}