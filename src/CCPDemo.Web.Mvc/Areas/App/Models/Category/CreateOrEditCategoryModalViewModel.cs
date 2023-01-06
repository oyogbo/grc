using CCPDemo.RiskCategory.Dtos;

using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Models.Category
{
    public class CreateOrEditCategoryModalViewModel
    {
        public CreateOrEditCategoryDto Category { get; set; }

        public bool IsEditMode => Category.Id.HasValue;
    }
}