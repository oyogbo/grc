using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.RiskTypes.Dtos
{
    public class CreateOrEditRiskTypeDto : EntityDto<int?>
    {

        public string Name { get; set; }

    }
}