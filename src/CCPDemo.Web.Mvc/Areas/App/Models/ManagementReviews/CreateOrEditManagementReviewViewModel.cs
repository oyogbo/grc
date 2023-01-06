using CCPDemo.ManagementReviews.Dtos;

using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Models.ManagementReviews
{
    public class CreateOrEditManagementReviewModalViewModel
    {
        public CreateOrEditManagementReviewDto ManagementReview { get; set; }

        public bool IsEditMode => ManagementReview.Id.HasValue;
    }
}