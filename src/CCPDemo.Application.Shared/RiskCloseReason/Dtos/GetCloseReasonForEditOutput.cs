using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.RiskCloseReason.Dtos
{
    public class GetCloseReasonForEditOutput
    {
        public CreateOrEditCloseReasonDto CloseReason { get; set; }

    }
}