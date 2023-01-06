using CCPDemo.Employee.Dtos;

using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Models.Employees
{
    public class CreateOrEditEmployeesModalViewModel
    {
        public CreateOrEditEmployeesDto Employees { get; set; }

        public bool IsEditMode => Employees.Id.HasValue;
    }
}