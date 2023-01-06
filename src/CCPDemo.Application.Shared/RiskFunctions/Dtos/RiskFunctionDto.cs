using System;
using Abp.Application.Services.Dto;

namespace CCPDemo.RiskFunctions.Dtos
{
    public class RiskFunctionDto : EntityDto
    {
        public int Value { get; set; }

        public string Name { get; set; }

    }
}