using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.RiskStatus.Dtos
{
    public class CreateOrEditStatusDto : EntityDto<int?>
    {

        [Required]
        public string Name { get; set; }

    }
}