using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.KeyRiskIndicators.Dtos
{
    public class GetKeyRiskIndicatorForEditOutput
    {
        public CreateOrEditKeyRiskIndicatorDto KeyRiskIndicator { get; set; }

    }
}