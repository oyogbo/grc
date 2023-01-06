using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.RiskFunctions.Dtos
{
    public class CreateOrEditRiskFunctionDto : EntityDto<int?>
    {

        public int Value { get; set; }

        [Required]
        [StringLength(RiskFunctionConsts.MaxNameLength, MinimumLength = RiskFunctionConsts.MinNameLength)]
        public string Name { get; set; }

    }
}