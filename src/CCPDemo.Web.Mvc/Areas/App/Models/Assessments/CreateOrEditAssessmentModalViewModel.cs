using CCPDemo.Assessments.Dtos;

using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Models.Assessments
{
    public class CreateOrEditAssessmentModalViewModel
    {
        public CreateOrEditAssessmentDto Assessment { get; set; }

        public bool IsEditMode => Assessment.Id.HasValue;
    }
}