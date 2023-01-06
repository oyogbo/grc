using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.RiskFunctions.Dtos
{
    public class GetRiskFunctionForEditOutput
    {
        public CreateOrEditRiskFunctionDto RiskFunction { get; set; }

    }
}