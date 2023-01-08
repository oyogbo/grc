using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.RiskStatuses.Dtos
{
    public class GetRiskStatusForEditOutput
    {
        public CreateOrEditRiskStatusDto RiskStatus { get; set; }

    }
}