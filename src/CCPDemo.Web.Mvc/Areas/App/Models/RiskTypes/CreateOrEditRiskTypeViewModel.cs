using CCPDemo.RiskTypes.Dtos;

using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Models.RiskTypes
{
    public class CreateOrEditRiskTypeModalViewModel
    {
        public CreateOrEditRiskTypeDto RiskType { get; set; }

        public bool IsEditMode => RiskType.Id.HasValue;
    }
}