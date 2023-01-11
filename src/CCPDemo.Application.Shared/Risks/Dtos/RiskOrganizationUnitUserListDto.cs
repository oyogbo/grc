using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCPDemo.Risks.Dtos
{
    public class RiskOrganizationUnitUserListDto : EntityDto<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public Guid? ProfilePictureId { get; set; }

        public DateTime AddedTime { get; set; }
    }
}
