using CCPDemo.VRisks.Dtos;

using Abp.Extensions;
using System.Collections.Generic;
using CCPDemo.Authorization.Users;

namespace CCPDemo.Web.Areas.App.Models.VRisks
{
    public class CreateOrEditVRiskModalViewModel
    {
        public CreateOrEditVRiskDto VRisk { get; set; }

        public bool IsEditMode => VRisk.Id.HasValue;

        public IList<User> Users { get; set; }
        public User User { get; set; }
       
    }
}