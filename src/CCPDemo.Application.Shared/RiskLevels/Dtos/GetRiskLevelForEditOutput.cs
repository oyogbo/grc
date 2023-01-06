using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.RiskLevels.Dtos
{
    public class GetRiskLevelForEditOutput
    {
        public CreateOrEditRiskLevelDto RiskLevel { get; set; }

    }
}