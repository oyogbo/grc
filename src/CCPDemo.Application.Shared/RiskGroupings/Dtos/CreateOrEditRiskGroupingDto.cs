using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.RiskGroupings.Dtos
{
    public class CreateOrEditRiskGroupingDto : EntityDto<int?>
    {

        public int Value { get; set; }

        [Required]
        [StringLength(RiskGroupingConsts.MaxNameLength, MinimumLength = RiskGroupingConsts.MinNameLength)]
        public string Name { get; set; }

        public short Default { get; set; }

        public int Order { get; set; }

    }
}