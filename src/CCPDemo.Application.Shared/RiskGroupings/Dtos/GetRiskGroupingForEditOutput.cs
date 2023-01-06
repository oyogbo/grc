using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.RiskGroupings.Dtos
{
    public class GetRiskGroupingForEditOutput
    {
        public CreateOrEditRiskGroupingDto RiskGrouping { get; set; }

    }
}