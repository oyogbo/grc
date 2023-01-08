using CCPDemo.RiskStatus.Dtos;

using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Models.Status
{
    public class CreateOrEditStatusModalViewModel
    {
        public CreateOrEditStatusDto Status { get; set; }

        public bool IsEditMode => Status.Id.HasValue;
    }
}