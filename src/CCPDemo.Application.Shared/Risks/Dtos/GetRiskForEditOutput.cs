using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.Risks.Dtos
{
    public class GetRiskForEditOutput
    {
        public CreateOrEditRiskDto Risk { get; set; }

        public string RiskTypeName { get; set; }

        public string OrganizationUnitDisplayName { get; set; }

        public string StatusName { get; set; }

        public string RiskRatingName { get; set; }

        public string UserName { get; set; }

    }
}