using CCPDemo.Closures.Dtos;

using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Models.Closures
{
    public class CreateOrEditClosureModalViewModel
    {
        public CreateOrEditClosureDto Closure { get; set; }

        public bool IsEditMode => Closure.Id.HasValue;
    }
}