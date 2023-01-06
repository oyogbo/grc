using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.Regulations.Dtos
{
    public class CreateOrEditRegulationDto : EntityDto<int?>
    {

        [Required]
        public int value { get; set; }

        [Required]
        [StringLength(RegulationConsts.MaxnameLength, MinimumLength = RegulationConsts.MinnameLength)]
        public string name { get; set; }

    }
}