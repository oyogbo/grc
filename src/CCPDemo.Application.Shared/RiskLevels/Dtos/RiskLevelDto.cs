using System;
using Abp.Application.Services.Dto;

namespace CCPDemo.RiskLevels.Dtos
{
    public class RiskLevelDto : EntityDto
    {
        public decimal Value { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public string DisplayName { get; set; }

    }
}