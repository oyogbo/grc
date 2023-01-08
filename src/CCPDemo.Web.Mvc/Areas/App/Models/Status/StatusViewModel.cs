using CCPDemo.RiskStatus.Dtos;

namespace CCPDemo.Web.Areas.App.Models.Status
{
    public class StatusViewModel : GetStatusForViewDto
    {
        public string FilterText { get; set; }
    }
}