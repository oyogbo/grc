using System;
using Abp.Application.Services.Dto;

namespace CCPDemo.RiskCloseReason.Dtos
{
    public class CloseReasonDto : EntityDto
    {
        public int value { get; set; }

        public string name { get; set; }

    }
}