using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.RiskModels.Dtos
{
    public class CreateOrEditRiskModelDto : EntityDto<int?>
    {

        public int Value { get; set; }

        [Required]
        [StringLength(RiskModelConsts.MaxNameLength, MinimumLength = RiskModelConsts.MinNameLength)]
        public string Name { get; set; }

    }
}