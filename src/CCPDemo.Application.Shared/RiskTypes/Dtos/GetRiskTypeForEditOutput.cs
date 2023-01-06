using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.RiskTypes.Dtos
{
    public class GetRiskTypeForEditOutput
    {
        public CreateOrEditRiskTypeDto RiskType { get; set; }

    }
}