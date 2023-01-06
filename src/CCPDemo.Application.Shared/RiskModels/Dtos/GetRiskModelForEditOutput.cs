using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.RiskModels.Dtos
{
    public class GetRiskModelForEditOutput
    {
        public CreateOrEditRiskModelDto RiskModel { get; set; }

    }
}