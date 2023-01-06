using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.RiskLevels.Dtos
{
    public class CreateOrEditRiskLevelDto : EntityDto<int?>
    {

        public decimal Value { get; set; }

        [Required]
        [StringLength(RiskLevelConsts.MaxNameLength, MinimumLength = RiskLevelConsts.MinNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(RiskLevelConsts.MaxColorLength, MinimumLength = RiskLevelConsts.MinColorLength)]
        public string Color { get; set; }

        [Required]
        [StringLength(RiskLevelConsts.MaxDisplayNameLength, MinimumLength = RiskLevelConsts.MinDisplayNameLength)]
        public string DisplayName { get; set; }

    }
}