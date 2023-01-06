using CCPDemo.RiskCloseReason.Dtos;

using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Models.CloseReasons
{
    public class CreateOrEditCloseReasonModalViewModel
    {
        public CreateOrEditCloseReasonDto CloseReason { get; set; }

        public bool IsEditMode => CloseReason.Id.HasValue;
    }
}