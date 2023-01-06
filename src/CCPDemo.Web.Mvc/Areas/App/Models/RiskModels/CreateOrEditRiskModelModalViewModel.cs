using CCPDemo.RiskModels.Dtos;

using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Models.RiskModels
{
    public class CreateOrEditRiskModelModalViewModel
    {
        public CreateOrEditRiskModelDto RiskModel { get; set; }

        public bool IsEditMode => RiskModel.Id.HasValue;
    }
}