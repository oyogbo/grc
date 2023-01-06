using System;
using Abp.Application.Services.Dto;

namespace CCPDemo.RiskGroupings.Dtos
{
    public class RiskGroupingDto : EntityDto
    {
        public int Value { get; set; }

        public string Name { get; set; }

        public short Default { get; set; }

        public int Order { get; set; }

    }
}