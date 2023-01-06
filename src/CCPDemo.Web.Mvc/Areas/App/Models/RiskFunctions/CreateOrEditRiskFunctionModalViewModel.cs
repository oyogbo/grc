using CCPDemo.RiskFunctions.Dtos;

using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Models.RiskFunctions
{
    public class CreateOrEditRiskFunctionModalViewModel
    {
        public CreateOrEditRiskFunctionDto RiskFunction { get; set; }

        public bool IsEditMode => RiskFunction.Id.HasValue;
    }
}