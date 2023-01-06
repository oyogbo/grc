using System;
using Abp.Application.Services.Dto;

namespace CCPDemo.RiskModels.Dtos
{
    public class RiskModelDto : EntityDto
    {
        public int Value { get; set; }

        public string Name { get; set; }

    }
}