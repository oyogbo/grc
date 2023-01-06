using System;
using Abp.Application.Services.Dto;

namespace CCPDemo.Regulations.Dtos
{
    public class RegulationDto : EntityDto
    {
        public int value { get; set; }

        public string name { get; set; }

    }
}