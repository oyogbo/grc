using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.RiskStatus.Dtos
{
    public class GetStatusForEditOutput
    {
        public CreateOrEditStatusDto Status { get; set; }

    }
}