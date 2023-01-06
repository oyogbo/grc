using CCPDemo.Projects.Dtos;

using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Models.Projects
{
    public class CreateOrEditProjectModalViewModel
    {
        public CreateOrEditProjectDto Project { get; set; }

        public bool IsEditMode => Project.Id.HasValue;
    }
}