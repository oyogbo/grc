using CCPDemo.RiskRatings.Dtos;

using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Models.RiskRatings
{
    public class CreateOrEditRiskRatingModalViewModel
    {
        public CreateOrEditRiskRatingDto RiskRating { get; set; }

        public bool IsEditMode => RiskRating.Id.HasValue;
    }
}