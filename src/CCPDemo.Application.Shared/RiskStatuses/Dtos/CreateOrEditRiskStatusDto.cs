using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.RiskStatuses.Dtos
{
    public class CreateOrEditRiskStatusDto : EntityDto<int?>
    {

        [Required]
        public string Name { get; set; }

    }
}