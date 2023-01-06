using CCPDemo.Departments.Dtos;

using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Models.Departments
{
    public class CreateOrEditDepartmentModalViewModel
    {
        public CreateOrEditDepartmentDto Department { get; set; }

        public bool IsEditMode => Department.Id.HasValue;
    }
}