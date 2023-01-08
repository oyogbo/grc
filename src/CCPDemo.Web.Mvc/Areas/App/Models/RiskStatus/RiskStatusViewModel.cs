using CCPDemo.RiskStatuses.Dtos;

namespace CCPDemo.Web.Areas.App.Models.RiskStatus
{
    public class RiskStatusViewModel : GetRiskStatusForViewDto
    {
        public string FilterText { get; set; }
    }
}