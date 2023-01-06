using CCPDemo.ReviewLevels.Dtos;

using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Models.ReviewLevels
{
    public class CreateOrEditReviewLevelModalViewModel
    {
        public CreateOrEditReviewLevelDto ReviewLevel { get; set; }

        public bool IsEditMode => ReviewLevel.Id.HasValue;
    }
}