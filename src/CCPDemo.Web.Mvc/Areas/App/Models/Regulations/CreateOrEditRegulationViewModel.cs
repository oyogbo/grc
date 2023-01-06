using CCPDemo.Regulations.Dtos;

using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Models.Regulations
{
    public class CreateOrEditRegulationModalViewModel
    {
        public CreateOrEditRegulationDto Regulation { get; set; }

        public bool IsEditMode => Regulation.Id.HasValue;
    }
}