using CCPDemo.UsersLookups.Dtos;

using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Models.UsersLookups
{
    public class CreateOrEditUsersLookupViewModel
    {
        public CreateOrEditUsersLookupDto UsersLookup { get; set; }

        public string UserName { get; set; }

        public bool IsEditMode => UsersLookup.Id.HasValue;
    }
}