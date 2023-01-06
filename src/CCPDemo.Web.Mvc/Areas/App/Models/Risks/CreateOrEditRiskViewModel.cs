using CCPDemo.Risks.Dtos;

using Abp.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using CCPDemo.Authorization.Users;

namespace CCPDemo.Web.Areas.App.Models.Risks
{
    public class CreateOrEditRiskModalViewModel
    {
        public CreateOrEditRiskDto Risk { get; set; }

        public bool IsEditMode => Risk.Id.HasValue;

        public List<SelectListItem> RiskTypeList { get; set; }
        public List<SelectListItem> UsersList { get; set; }

        public IDictionary<string, long> UsersMap { get; set; }

        public List<User> Users { get; set; }
        public User User { get; set; }
    }
}