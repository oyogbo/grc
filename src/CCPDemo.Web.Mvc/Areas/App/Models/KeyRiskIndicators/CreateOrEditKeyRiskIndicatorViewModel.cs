using CCPDemo.KeyRiskIndicators.Dtos;

using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Models.KeyRiskIndicators
{
    public class CreateOrEditKeyRiskIndicatorModalViewModel
    {
        public CreateOrEditKeyRiskIndicatorDto KeyRiskIndicator { get; set; }

        public bool IsEditMode => KeyRiskIndicator.Id.HasValue;
    }
}