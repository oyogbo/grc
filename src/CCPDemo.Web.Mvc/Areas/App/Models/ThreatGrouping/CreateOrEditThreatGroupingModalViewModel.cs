using CCPDemo.ThreatGroupings.Dtos;

using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Models.ThreatGrouping
{
    public class CreateOrEditThreatGroupingModalViewModel
    {
        public CreateOrEditThreatGroupingDto ThreatGrouping { get; set; }

        public bool IsEditMode => ThreatGrouping.Id.HasValue;
    }
}